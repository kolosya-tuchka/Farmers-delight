using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPShooting : Shooting
{
    string mpPath = "Multiplayer/Bullets/";

    void Start()
    {
        gun = GetComponent<Gun>();
    }

    public override void Attack()
    {
        if (!gun.canAttack || gun.isReloading) return;

        var _bullet = PhotonNetwork.Instantiate(mpPath+bullet.name, gameObject.transform.position + gameObject.transform.right / 2, gameObject.transform.rotation);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        gun.canAttack = false;
        gun.curAmmo--;
        shotFeedback.PlayFeedbacks(transform.position);
    }
}
