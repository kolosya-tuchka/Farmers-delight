using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPShooting : Shooting
{
    string mpPath = "Multiplayer/Bullets/";

    void Start()
    {
        if (!GetComponent<PhotonView>().IsMine) return;
        gun = GetComponent<Gun>();
        defWeapon = gun.owner.GetComponent<DefaultMeleeWeapon>();
    }

    public override void Attack()
    {
        var enemy = Physics2D.OverlapCircle(transform.position + (Vector3)defWeapon.attackRangeCenter, defWeapon.attackRange, gun.owner.GetComponent<PlayerController>().enemyMask);
        if (enemy != null)
        {
            defWeapon.GetComponent<IAttack>().Attack();
            gun.canAttack = false;
        }

        if (!gun.canAttack || gun.isReloading) return;

        var _bullet = PhotonNetwork.Instantiate(mpPath+bullet.name, gameObject.transform.position + gameObject.transform.right / 2, gameObject.transform.rotation);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        gun.canAttack = false;
        gun.curAmmo--;
        shotFeedback.PlayFeedbacks(transform.position);
    }
}
