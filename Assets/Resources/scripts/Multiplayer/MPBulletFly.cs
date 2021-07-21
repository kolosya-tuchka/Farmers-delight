using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPBulletFly : BulletFly
{
    Rigidbody2D rigidbody;
    Bullet bullet;

    Enemy enemy;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        bullet = gameObject.GetComponent<Bullet>();

        rigidbody.velocity = transform.right * speed;
        StartCoroutine(DestroyAfterTime(bullet.gameObject, bullet.timeOfFlying));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.CompareTag("Construction") || collision.gameObject.CompareTag("DefObj"))
        {
            var part = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            enemy = collision.GetComponent<Enemy>();
            enemy.behaviour = Enemy.Behaviour.attacker;
            enemy.hp.healPoints -= bullet.damage;
            var dir = enemy.GetComponent<MPDirectioner>();
            enemy.GetComponent<EnemyAnimations>().hit = true;

            if (enemy.hp.healPoints <= 0)
            {
                if (GetComponent<PhotonView>().IsMine)
                {
                    dir.killer = dir.mp.player.GetComponent<Player>();
                }

                collision.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(DestroyAfterTime(enemy.gameObject, 30));
                dir.StopAllCoroutines();
            }

            var part = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        PhotonNetwork.Destroy(obj);
    }

}
