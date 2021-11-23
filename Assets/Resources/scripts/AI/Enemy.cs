using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Die(Player player);
    void Die();

}

public class Enemy : MonoBehaviour, IEnemy
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
        var player = FindObjectOfType<Player>();

        Die(player);
    }

    public virtual void Die(Player player)
    {
        var agent = GetComponent<NavMeshAgent2D>();
        var rig = GetComponent<Rigidbody2D>();
        var manager = FindObjectOfType<EnemyManager>();
        player.kills++;
        agent.enabled = false;
        rig.velocity = Vector2.zero;
        manager.enemiesOnSceneNow--;
        state = Enemy.State.dead;
    }

}
