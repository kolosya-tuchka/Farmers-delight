using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    Player pl;
    PlayerController pc;
    Rigidbody2D rb;

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>().gameObject;
        pl = player.GetComponent<Player>();
        pc = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 point = player.transform.position;
        var weapon = pl.weapons[pl.currentGun];
        float speed = 0;

        if (pc.targetEnemy == null)
        {
            point += rb.velocity * 1.5f;
            speed = 0.01f;
        }
        else
        {
            point += (Vector2)(pc.targetEnemy.transform.position - player.transform.position) * 2 / 3;
            speed = 0.05f;
        }
        transform.transform.position = Vector2.Lerp(transform.position, point, speed);
    }
}
