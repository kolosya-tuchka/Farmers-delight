using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy, IEnemy
{
    public override void Die(Player player)
    {
        if (state == State.dead) return;
        
        var agent = GetComponent<NavMeshAgent2D>();
        var rig = GetComponent<Rigidbody2D>();
        var manager = FindObjectOfType<BossManager>();
        var dir = GetComponent<Directioner>();
        
        agent.enabled = false;
        rig.velocity = Vector2.zero;
        dir.Drop();
        dir.StopAllCoroutines();
        manager.curBossCount--;
        GetComponent<Collider2D>().enabled = false;
        state = Enemy.State.dead;
        
        if (player != null)
        {
            player.kills++;
        }
    }
}
