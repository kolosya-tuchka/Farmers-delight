using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidbody;

    [HideInInspector]
    public Bullet bullet;

    public int speed;
    public GameObject particle;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        bullet = gameObject.GetComponent<Bullet>();

        GameObject.Destroy(gameObject, bullet.timeOfFlying);

        rigidbody.velocity = transform.right * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Construction") || collision.gameObject.CompareTag("DefObj"))
        {
            var part = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            collision.GetComponent<Directioner>().TakeDamage(bullet.damage);
            var part = Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }

}
