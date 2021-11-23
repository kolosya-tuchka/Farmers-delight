using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy, IEnemy
{
    public override void Die(Player player)
    {
        var agent = GetComponent<NavMeshAgent2D>();
        var rig = GetComponent<Rigidbody2D>();
        var manager = FindObjectOfType<BossManager>();
        var dir = GetComponent<Directioner>();

        player.kills++;
        agent.enabled = false;
        rig.velocity = Vector2.zero;
        dir.Drop();
        manager.curBossCount--;
        state = Enemy.State.dead;
    }
}
