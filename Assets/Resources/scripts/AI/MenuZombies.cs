using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuZombies : MonoBehaviour
{
    float velChange = 0;
    Rigidbody2D rigidbody;
    Animator animator;
    SpriteRenderer renderer;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        GameObject.Destroy(gameObject, Random.Range(30, 40));
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        velChange -= Time.deltaTime;
        if (velChange <= 0)
        {
            rigidbody.velocity = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)).normalized  * 3;
            velChange = 1;
        }
        animator.SetBool("IsMoving", rigidbody.velocity != Vector2.zero);
        renderer.flipX = rigidbody.velocity.x < 0;
    }
}
