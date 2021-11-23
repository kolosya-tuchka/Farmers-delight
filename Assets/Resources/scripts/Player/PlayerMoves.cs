using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoves : MonoBehaviour
{
    protected PlayerController controls;
    protected Player player;
    protected Rigidbody2D rb;

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        CheckMove();
        PlayerRotation();
        CheckForTarget();
    }

    public virtual void PlayerRotation()
    {
        var weapon = player.weapons[player.currentGun];

        if (controls.targetEnemy != null)
            player.rend.flipX = weapon.transform.eulerAngles.z > 90 && weapon.transform.eulerAngles.z < 270;
        else if (rb.velocity.x < 0)
            player.rend.flipX = true;
        else if (rb.velocity.x > 0)
            player.rend.flipX = false;
    }

    public virtual void CheckForTarget()
    {
        if (controls.targetEnemy == null)
        {
            controls.targetEnemy = controls.GetEnemyNearby(player.focusDistance);
            return;
        }

        if (Vector2.Distance(transform.position, controls.targetEnemy.transform.position) > player.focusDistance + 1)
        {
            controls.targetEnemy = null;
        }
        else if (controls.targetEnemy.state == Enemy.State.dead)
        {
            controls.targetEnemy = null;
        }
    }


    protected virtual void CheckMove()
    {
        float x = 0, y = 0;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            x = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            x = -1;
        }


        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            y = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            y = -1;
        }

        SetVelocity(new Vector2(x, y));
        player.anim.MoveAnimation(rb.velocity);
    }

    protected void SetVelocity(Vector2 mvm)
    {
        rb.velocity = mvm.normalized * player.speed * player.speedBoost;
    }

    protected void OnStart()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = GetComponent<PlayerController>();
        player = GetComponent<Player>();
    }
}
