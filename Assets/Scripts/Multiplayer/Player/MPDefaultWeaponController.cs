using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPDefaultWeaponController : DefaultMeleeWeaponController, IAttack
{
    private PhotonView view;
    void Awake()
    {
        OnStart();
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            enabled = false;
        }
        weapon = GetComponent<DefaultMeleeWeapon>();
    }

    void Update()
    {
        FixAnim();
    }

    public override void Attack()
    {
        if (!weapon.canAttack) return;

        var enemy = Physics2D.OverlapCircle(transform.position + (Vector3)weapon.attackRangeCenter, weapon.attackRange, player.GetComponent<PlayerController>().enemyMask)?.GetComponent<Enemy>();
        if (enemy?.state == Enemy.State.dead)
        {
            return;
        }
        enemy?.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, weapon.damage, MPManager.PlayerIndex(GetComponent<PhotonView>().Owner));
        weapon.anim.SetTrigger("Attack");
        weapon.canAttack = false;
    }

    private void OnEnable()
    {
        StartCoroutine(DelayCheck());
    }
}
