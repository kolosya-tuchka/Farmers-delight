using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    Rigidbody2D rigidbody;
    public int speed;
    Bullet bullet;
    PlayerManager playerManager;
    public GameObject particle;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        bullet = gameObject.GetComponent<Bullet>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();

        GameObject.Destroy(gameObject, bullet.timeOfFlying);
    }

    void Update()
    {
        rigidbody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Construction") || collision.gameObject.CompareTag("DefObj"))
        {
            var part = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            var enemy = collision.GetComponent<Enemy>();
            enemy.behaviour = Enemy.Behaviour.attacker;
            enemy.hp.healPoints -= bullet.damage;
            var dir = enemy.GetComponent<Directioner>();
            enemy.GetComponent<EnemyAnimations>().hit = true;
            if (enemy.hp.healPoints <= 0)
            {
                collision.GetComponent<BoxCollider2D>().enabled = false;
                GameObject.Destroy(enemy.gameObject, 30);
                dir.StopAllCoroutines();
            }
            var part = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }

}
