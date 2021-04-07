using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HP hp;
    public float damage, seeDistance, attackDistance, speed, maxSeeDistance, repRate;
    public bool canDestroy;
    public Behaviour behaviour;
    public int coins;
    public State state;
    public enum Behaviour : int
    {
        attacker, destroyer
    }

    public enum State
    {
        alive, dead
    }

    public void RandomBeh()
    {
        int beh = UnityEngine.Random.Range(1, 5);
        if (beh == 4) behaviour = Enemy.Behaviour.destroyer;
        else behaviour = Enemy.Behaviour.attacker;
    }

    public void Die()
    {
        var agent = GetComponent<NavMeshAgent2D>();
        var rig = GetComponent<Rigidbody2D>();
        var manager = FindObjectOfType<EnemyManager>();
        var player = GameObject.Find("Player").GetComponent<Player>();
        var dir = GetComponent<Directioner>();
        player.kills++;
        agent.enabled = false;
        rig.velocity = Vector2.zero;
        manager.enemiesOnSceneNow--;
        var rounds = manager.gameObject.GetComponent<RoundManager>();
        if (tag == "Boss")
        {
            //rounds.isBreak = true;
            //foreach (var en in GameObject.FindGameObjectsWithTag("Enemy"))
            //{
            //    GameObject.Destroy(en);
            //}
            dir.Drop();
        }
        if (rounds.roundType == RoundManager.RoundType.simple)
        {
           dir.Drop();
        }
        state = Enemy.State.dead;
    }
}
