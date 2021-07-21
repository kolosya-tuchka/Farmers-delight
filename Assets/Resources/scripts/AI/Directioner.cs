using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Directioner : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D dir;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public NavMeshAgent2D agent;
    [HideInInspector] public float repRate, timeOfPersuit, maxTimeOfPersuit;
    [HideInInspector] public GameObject obj, def;
    public SpriteRenderer rend;
    [HideInInspector] public EnemyAnimations anim;
    [HideInInspector] public EnemyManager manager;
    void Start()
    {
        dir = gameObject.GetComponent<Rigidbody2D>();

        enemy = gameObject.GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent2D>();
        agent.speed = enemy.speed;
        agent.stoppingDistance = enemy.attackDistance;
        enemy.RandomBeh();
        enemy.state = Enemy.State.alive;


        def = FindObjectOfType<DefObj>().gameObject;

        anim = GetComponent<EnemyAnimations>();

        if (enemy.behaviour == Enemy.Behaviour.attacker) maxTimeOfPersuit = 10;
        StartCoroutine(Move());

        manager = GameObject.Find("Game Manager").GetComponent<EnemyManager>();

        if (CompareTag("Enemy"))
        {
            enemy.coins += Random.Range(-3, 6);
            repRate =enemy.repRate + Random.Range(-0.1f, 0.1f);
        }
    }

    private void FixedUpdate()
    {
        obj = FindObjectOfType<Player>().gameObject;
        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead)
        {
            enemy.Die();
        }
        Check();
    }

    public IEnumerator Move()
    {
        while (enemy.hp.healPoints > 0)
        {
            yield return new WaitForSeconds(repRate);
            repRate = enemy.repRate;
            PathToDef();
            PathToPlayer();
        }
    }

    public virtual void Attack()
    {
        rend.flipX = transform.position.x > obj.transform.position.x;
        var player = obj.GetComponent<HP>();
        player.healPoints -= enemy.damage;
        player.delayTimeLeft = player.delayOfRegeneration;
        anim.isAttack = true;
        timeOfPersuit = 0;
    }

    public virtual void Drop()
    {
        var coins = manager.GetComponent<Coins>();
        GameObject coin;
        while (enemy.coins > 0)
        {
            if (enemy.coins >= 10)
            {
                coin = Instantiate(coins.coins[0], transform.position, Quaternion.identity);
                enemy.coins -= 10;
            }
            else if (enemy.coins >= 5)
            {
                coin = Instantiate(coins.coins[1], transform.position, Quaternion.identity);
                enemy.coins -= 5;
            }
            else
            {
                coin = Instantiate(coins.coins[2], transform.position, Quaternion.identity);
                enemy.coins -= 1;
            }
            coin.transform.parent = GameObject.Find("Coins").transform;
        }
    }

    public virtual void PathToDef()
    {
        if (enemy.behaviour == Enemy.Behaviour.destroyer && def != null)
        {
            if (Vector2.Distance(obj.transform.position, gameObject.transform.position) <= enemy.seeDistance) enemy.behaviour = Enemy.Behaviour.attacker;
            agent.destination = def.transform.position;
            if (enemy.canDestroy)
            {
                anim.isAttack = true;
                var defHP = def.GetComponent<HP>();
                defHP.delayTimeLeft = defHP.delayOfRegeneration;
                repRate = 1.5f;
                def.GetComponent<DefObj>().hp.healPoints -= enemy.damage;
                timeOfPersuit = 0;
                enemy.canDestroy = false;
            }
            rend.flipX = agent.destination.x < transform.position.x;
        }
    }

    public virtual void PathToPlayer()
    {
        if (enemy.behaviour == Enemy.Behaviour.attacker || def == null)
        {
            agent.destination = obj.transform.position;
            if (Vector2.Distance(obj.transform.position, gameObject.transform.position) <= enemy.attackDistance)
            {
                Attack();
                repRate = 1f;
            }
            else if (agent.destination != null) rend.flipX = agent.destination.x < transform.position.x;
            else rend.flipX = dir.velocity.x < 0;
        }
    }

    public void Check()
    {

        timeOfPersuit += Time.deltaTime;
        if (Vector2.Distance(obj.transform.position, gameObject.transform.position) > enemy.maxSeeDistance && timeOfPersuit >= maxTimeOfPersuit)
        {
            enemy.behaviour = Enemy.Behaviour.destroyer;
            timeOfPersuit = 0;
            maxTimeOfPersuit = 5;
        }
        else if (Vector2.Distance(obj.transform.position, gameObject.transform.position) <= enemy.seeDistance || timeOfPersuit >= maxTimeOfPersuit)
        {
            enemy.behaviour = Enemy.Behaviour.attacker;
            timeOfPersuit = 0;
            maxTimeOfPersuit = 5;
        }
    }
}
