using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Directioner, IDamage
{
    void Start()
    {
        enemy = GetComponent<Boss>();
        player = FindObjectOfType<Player>();
        enemy.coins += Random.Range(0, 11) * 10;
        iEnemy = GetComponent<Boss>();
        OnStart();
        StartCoroutine(Move());
    }

    public override IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(repRate);
            repRate = enemy.repRate;
            PathToPlayer();
        }
    }

    public override void TakeDamage(float damage)
    {
        enemy.hp.healPoints -= damage;
        anim.hit = true;
        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead)
        {
            iEnemy.Die(player);
            Drop();
            GetComponent<BoxCollider2D>().enabled = false;
            Destroy(this);
            Destroy(gameObject, 30);
        }
    }

    public override void Attack()
    {
        base.Attack();
    }

}
