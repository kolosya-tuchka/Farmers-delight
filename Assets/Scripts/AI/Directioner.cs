using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(float damage);
}

public class Directioner : MonoBehaviour, IDamage
{
    protected Rigidbody2D dir;
    protected Enemy enemy;
    protected IEnemy iEnemy;
    protected NavMeshAgent2D agent;
    protected float repRate, timeOfPersuit, maxTimeOfPersuit;
    protected Player player;
    protected DefObj def;
    protected Transform defPoint;
    protected EnemyAnimations anim;
    protected EnemyManager manager;
    [SerializeField] protected SpriteRenderer rend;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.RandomBeh();

        def = FindObjectOfType<DefObj>();
        player = FindObjectOfType<Player>();

        if (enemy.behaviour == Enemy.Behaviour.attacker) maxTimeOfPersuit = 10;

        enemy.coins += Random.Range(-3, 6);
        iEnemy = GetComponent<Enemy>();
        OnStart();
        StartCoroutine(Move());
    }

    public virtual void OnStart()
    {
        dir = gameObject.GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent2D>();
        agent.speed = enemy.speed;
        agent.stoppingDistance = enemy.attackDistance;
        enemy.state = Enemy.State.alive;
        manager = FindObjectOfType<EnemyManager>();
        anim = GetComponent<EnemyAnimations>();
        defPoint = def.GetDestinationPoint();
    }

    private void FixedUpdate()
    {
        Check();
    }

    public virtual IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(repRate);
            repRate = enemy.repRate;
            PathToDef();
            PathToPlayer();
        }
    }

    public virtual void TakeDamage(float damage)
    {
        enemy.behaviour = Enemy.Behaviour.attacker;
        enemy.hp.healPoints -= damage;
        anim.hit = true;
        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead)
        {
            iEnemy.Die(player);
            var rounds = manager.GetComponent<RoundManager>();
            if (rounds.roundType == RoundManager.RoundType.simple)
            {
                Drop();
            }
            Destroy(this);
            Destroy(enemy.gameObject, 30);
        }
    }

    public virtual void Attack()
    {
        player.GetComponent<PlayerController>().TakeDamage(enemy.damage);
        rend.flipX = transform.position.x > player.transform.position.x;
        anim.isAttack = true;
        timeOfPersuit = 0;
    }

    public virtual void Drop()
    {
        var coins = manager.GetComponent<Coins>();
        int index = coins.coins.Length - 1;
        while (enemy.coins > 0)
        {
            int cost = coins.coins[index].GetComponent<CoinManager>().cost;
            while (enemy.coins >= cost)
            {
                Instantiate(coins.coins[index], transform.position, Quaternion.identity, coins.coinsParent.transform);
                enemy.coins -= cost;
            }
            --index;
        }
    }

    public virtual void PathToDef()
    {
        if (enemy.behaviour == Enemy.Behaviour.destroyer && def != null)
        {
            if (player != null && Vector2.Distance(player.transform.position, gameObject.transform.position) <= enemy.seeDistance)
                enemy.behaviour = Enemy.Behaviour.attacker;
            agent.destination = defPoint.position;
            if (enemy.canDestroy)
            {
                anim.isAttack = true;
                repRate = 1.5f;
                def.hp.delayTimeLeft = def.hp.delayOfRegeneration;
                def.hp.healPoints -= enemy.damage;
                timeOfPersuit = 0;
            }
            rend.flipX = agent.destination.x < transform.position.x;
        }
    }

    public virtual void PathToPlayer()
    {
        if (player == null) return;

        if (enemy.behaviour == Enemy.Behaviour.attacker || def == null)
        {
            agent.destination = player.transform.position;
            if (Vector2.Distance(player.transform.position, gameObject.transform.position) <= enemy.attackDistance)
            {
                Attack();
                repRate = 1f;
            }
            rend.flipX = agent.destination.x < transform.position.x;
        }
    }

    public void Check()
    {
        if (player == null) return;

        timeOfPersuit += Time.deltaTime;
        if (Vector2.Distance(player.transform.position, gameObject.transform.position) > enemy.maxSeeDistance && timeOfPersuit >= maxTimeOfPersuit)
        {
            enemy.behaviour = Enemy.Behaviour.destroyer;
            timeOfPersuit = 0;
            maxTimeOfPersuit = 5;
        }
        else if (Vector2.Distance(player.transform.position, gameObject.transform.position) <= enemy.seeDistance && timeOfPersuit >= maxTimeOfPersuit)
        {
            enemy.behaviour = Enemy.Behaviour.attacker;
            timeOfPersuit = 0;
            maxTimeOfPersuit = 5;
        }
    }
}
