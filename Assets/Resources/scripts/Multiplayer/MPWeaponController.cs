using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPWeaponController : WeaponController
{
    MPManager mp;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
        OnStart();
    }

    void Update()
    {
        Controll();
    }

    public override void Controll()
    {
        if (!view.IsMine) return;
        base.Controll();
    }

    public override void OnStart()
    {
        mp = FindObjectOfType<MPManager>();
        player = mp.player.GetComponent<Player>();
    }

}
