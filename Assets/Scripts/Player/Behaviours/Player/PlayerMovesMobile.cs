using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovesMobile : PlayerMoves
{
    [HideInInspector] public Joystick joystick;

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        CheckMove();
        CheckForTarget();
        PlayerRotation();
    }

    protected override void CheckMove()
    {
        Vector2 mvm = joystick.Direction.normalized;

        SetVelocity(mvm);
        player.anim.MoveAnimation(rb.velocity);
    }

}
