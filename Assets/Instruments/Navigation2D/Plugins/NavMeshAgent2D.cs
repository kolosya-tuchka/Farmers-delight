// Navigation2D Script (c) noobtuts.com
//
// NavMeshAgent2D is supposed to be a proxy to the original NavMeshAgent.
// All functions and properties should use get/set original agent.
// Any state we have could cause bugs, so we should not have state.
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class NavMeshAgent2D : MonoBehaviour
{
    // use the same serialized properties and pass them in awake once.
    // -> use the public getters/setters afterwards
    //    (we want to have virtually no extra state. just proxy functions.)
    [Header("Steering")]
    [SerializeField] float _speed = 3.5f;
    [SerializeField] float _angularSpeed = 120;
    [SerializeField] float _acceleration = 8;
    [SerializeField] float _stoppingDistance = 0;
    [SerializeField] bool _autoBraking = true; // false is too weird, true by default.

    // obstacle avoidance disabled by default because it just doesn't work very
    // well, not even in 3D. it's too confusing if users add a NavMeshAgent2D
    // and then encounter strange collisions. better use none.
    [Header("Obstacle Avoidance")]
    [SerializeField] float _radius = 0.5f;
    [SerializeField] ObstacleAvoidanceType _quality = ObstacleAvoidanceType.NoObstacleAvoidance;
    [SerializeField] int _priority = 50;

    [Header("Pathfinding")]
    [SerializeField] bool _autoRepath = true;

    // the projection
    NavMeshAgent agent;

    // cache
    new Rigidbody2D rigidbody2D;
    new Collider2D collider2D;

    // monobehaviour ///////////////////////////////////////////////////////////
    void Awake()
    {
        // create projection
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = "NAVIGATION2D_AGENT";
        go.transform.position = NavMeshUtils2D.ProjectTo3D(transform.position); // todo height 0.5 again?
        agent = go.AddComponent<NavMeshAgent>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        // disable navmesh and collider (no collider for now...)
        Destroy(agent.GetComponent<Collider>());
        Destroy(agent.GetComponent<MeshRenderer>());

        // pass serialized values once
        agent.speed = _speed;
        agent.angularSpeed = _angularSpeed;
        agent.acceleration = _acceleration;
        agent.stoppingDistance = _stoppingDistance;
        agent.autoBraking = _autoBraking;
        agent.radius = _radius;
        agent.obstacleAvoidanceType = _quality;
        agent.avoidancePriority = _priority;
        agent.autoRepath = _autoRepath;
    }

    bool IsStuck()
    {
        // stuck detection: get max distance first (best with collider)
        float maxdist = 2; // default if no collider
        if (collider2D != null)
        {
            var bounds = collider2D.bounds;
            maxdist = Mathf.Max(bounds.extents.x, bounds.extents.y) * 2;
        }

        // stuck detection: reset if distance > max distance
        float dist = Vector2.Distance(transform.position, NavMeshUtils2D.ProjectTo2D(agent.transform.position));
        return dist > maxdist;
    }

    void Update()
    {
        // copy position: transform in Update, rigidbody in FixedUpdate
        if (rigidbody2D == null || rigidbody2D.isKinematic)
            transform.position = NavMeshUtils2D.ProjectTo2D(agent.transform.position);

        // stuck detection
        if (IsStuck())
        {
            // stop agent movement, reset it to current position
            agent.ResetPath();
            agent.transform.position = NavMeshUtils2D.ProjectTo3D(transform.position);
            Debug.Log("stopped agent because of collision in 2D plane");
        }
    }

    void LateUpdate()
    {
        // copy position again, maybe NavMeshAgent did something new
        if (rigidbody2D == null || rigidbody2D.isKinematic)
            transform.position = NavMeshUtils2D.ProjectTo2D(agent.transform.position);
    }

    void FixedUpdate()
    {
        // copy position: transform in Update, rigidbody in FixedUpdate
        if (rigidbody2D != null && !rigidbody2D.isKinematic)
            rigidbody2D.MovePosition(NavMeshUtils2D.ProjectTo2D(agent.transform.position));
    }

    void OnDestroy()
    {
        if (agent != null) Destroy(agent.gameObject);
    }

    void OnEnable()
    {
        if (agent != null) agent.enabled = true;
    }

    void OnDisable()
    {
        if (agent != null) agent.enabled = false;
    }

    // draw radius gizmo (gizmos.matrix for correct rotation)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawWireSphere(Vector3.zero, agent != null ? radius : _radius);
    }

    // NavMeshAgent proxies ////////////////////////////////////////////////////
    public float acceleration
    {
        get { return agent.acceleration; }
        set { agent.acceleration = value; }
    }

    public float angularSpeed
    {
        get { return agent.angularSpeed; }
        set { agent.angularSpeed = value; }
    }

    public bool autoBraking
    {
        get { return agent.autoBraking; }
        set { agent.autoBraking = value; }
    }

    public bool autoRepath
    {
        get { return agent.autoRepath; }
        set { agent.autoRepath = value; }
    }

    public int avoidancePriority
    {
        get { return agent.avoidancePriority; }
        set { agent.avoidancePriority = value; }
    }

    public bool CalculatePath(Vector2 targetPosition, NavMeshPath2D path)
    {
        var temp = new NavMeshPath();
        if (agent.CalculatePath(NavMeshUtils2D.ProjectTo3D(targetPosition), temp))
        {
            // convert 3D to 2D
            path.corners = temp.corners.Select(NavMeshUtils2D.ProjectTo2D).ToArray();
            path.status = temp.status;
            return true;
        }
        return false;
    }

    public Vector2 destination
    {
        get { return NavMeshUtils2D.ProjectTo2D(agent.destination); }
        set { agent.destination = NavMeshUtils2D.ProjectTo3D(value); }
    }

    public bool hasPath
    {
        get { return agent.hasPath; }
    }

    public bool isOnNavMesh
    {
        get { return agent.isOnNavMesh; }
    }

    public bool isPathStale
    {
        get { return agent.isPathStale; }
    }

    public bool isStopped
    {
        get { return agent.isStopped; }
        set { agent.isStopped = value; }
    }

    public ObstacleAvoidanceType obstacleAvoidanceType
    {
        get { return agent.obstacleAvoidanceType; }
        set { agent.obstacleAvoidanceType = value; }
    }

    public NavMeshPath2D path
    {
        get
        {
            return new NavMeshPath2D()
            {
                corners = agent.path.corners.Select(NavMeshUtils2D.ProjectTo2D).ToArray(),
                status = agent.path.status
            };
        }
    }

    public bool pathPending
    {
        get { return agent.pathPending; }
    }

    public NavMeshPathStatus pathStatus
    {
        get { return agent.pathStatus; }
    }

    public float radius
    {
        get { return agent.radius; }
        set { agent.radius = value; }
    }

    public float remainingDistance
    {
        get { return agent.remainingDistance; }
    }

    public void ResetPath()
    {
        agent.ResetPath();
    }

    public void SetDestination(Vector2 v)
    {
        destination = v;
    }

    public float speed
    {
        get { return agent.speed; }
        set { agent.speed = value; }
    }

    public float stoppingDistance
    {
        get { return agent.stoppingDistance; }
        set { agent.stoppingDistance = value; }
    }

    // we set transform.position to agent.position in each Update, but if we
    // need the 100% 'true position right now' then we can also use this one:
    // (this was important for uMMORPG 2D)
    public Vector2 truePosition
    {
        get { return NavMeshUtils2D.ProjectTo2D(agent.transform.position); }
    }

    public Vector2 velocity
    {
        get { return NavMeshUtils2D.ProjectTo2D(agent.velocity); }
        set { agent.velocity = NavMeshUtils2D.ProjectTo3D(value); }
    }

    public void Warp(Vector2 v)
    {
        // try to warp, set this agent's position immediately if it worked, so
        // that Update doesn't cause issues when trying to move the rigidbody to
        // a far away position etc.
        if (agent.Warp(NavMeshUtils2D.ProjectTo3D(v)))
            transform.position = v;
    }
}
