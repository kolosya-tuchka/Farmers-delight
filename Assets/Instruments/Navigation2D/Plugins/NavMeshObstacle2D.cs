// Navigation2D Script (c) noobtuts.com
using UnityEngine;
using UnityEngine.AI;

public class NavMeshObstacle2D : MonoBehaviour
{
    // NavMeshObstacle properties
    public NavMeshObstacleShape shape = NavMeshObstacleShape.Box;
    public Vector2 center;
    public Vector2 size = Vector2.one;
    public bool carve = false; // experimental and hard to debug in 2D

    // the projection
    NavMeshObstacle obstacle;

    // monobehaviour ///////////////////////////////////////////////////////////
    void Awake()
    {
        // create projection
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = "NAVIGATION2D_OBSTACLE";
        go.transform.position = NavMeshUtils2D.ProjectTo3D(transform.position);
        go.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(transform.eulerAngles));
        obstacle = go.AddComponent<NavMeshObstacle>();

        // disable mesh and collider (no collider for now)
        Destroy(obstacle.GetComponent<Collider>());
        Destroy(obstacle.GetComponent<MeshRenderer>());
    }

    void Update()
    {
        // copy properties to projection all the time
        // (in case they are modified after creating it)
        obstacle.carving = carve;
        obstacle.center = NavMeshUtils2D.ProjectTo3D(center);
        obstacle.size = new Vector3(size.x, 1, size.y);

        // scale and rotate to match scaled/rotated sprites center properly
        obstacle.transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.y);
        obstacle.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(transform.eulerAngles));

        // project position to 3d
        obstacle.transform.position = NavMeshUtils2D.ProjectTo3D(transform.position);
    }

    void OnDestroy()
    {
        // destroy projection if not destroyed yet
        if (obstacle) Destroy(obstacle.gameObject);
    }

    void OnEnable()
    {
        if (obstacle) obstacle.enabled = true;
    }

    void OnDisable()
    {
        if (obstacle) obstacle.enabled = false;
    }

    // radius gizmo (gizmos.matrix for correct rotation)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(center, size);
    }

    // validation
    void OnValidate()
    {
        // force shape to box for now because we would need a separate Editor
        // GUI script to switch between size and radius otherwise
        shape = NavMeshObstacleShape.Box;
    }

    // NavMeshAgent proxies ////////////////////////////////////////////////////
    public Vector2 velocity
    {
        get { return NavMeshUtils2D.ProjectTo2D(obstacle.velocity); }
        // set: is a bad idea
    }
}
