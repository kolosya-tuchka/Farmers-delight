using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPMeleeAttack : MeleeAttack
{
    void Start()
    {
        weapon = GetComponent<MeleeWeapon>();
        player = weapon.owner;
    }

    public override void Attack()
    {
        if (!weapon.canAttack) return;

        var enemies = Physics2D.OverlapCircleAll(transform.position + weapon.offset, weapon.attackRange, player.GetComponent<MPPlayerController>().enemyMask);

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().state == Enemy.State.dead)
            {
                continue;
            }
            enemy.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, weapon.damage, MPManager.PlayerIndex(GetComponent<PhotonView>().Owner));
        }
        weapon.weaponAnimations.StartCoroutine(weapon.weaponAnimations.RotateWeapon(45, 1 / weapon.attackRate));
        weapon.anim.transform.position = transform.position + weapon.offset;
        weapon.anim.transform.rotation = transform.rotation;
        weapon.anim.SetTrigger("Attack");
        weapon.isFocused = true;
        weapon.canAttack = false;
        attackFeedback.PlayFeedbacks(transform.position);
    }
}
