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
        var owner = mp.player.GetComponent<PhotonView>().Owner;
        //GetComponent<PhotonView>().TransferOwnership(owner);

    }

    void Update()
    {
        Controll();
    }

    public override void Controll()
    {
        if (!GetComponent<PhotonView>().IsMine) return;
        base.Controll();
    }

    public override void OnStart()
    {
        mp = FindObjectOfType<MPManager>();
        player = mp.player;
        //joystick = GameObject.Find("Gun Joystick").GetComponent<Joystick>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    [PunRPC]
    void Sync(int playerIndex)
    {
        mp = FindObjectOfType<MPManager>();
        var player = mp.players[playerIndex].GetComponent<Player>();
        int i = 0;
        bool placed = false;
        foreach (var g in player.guns)
        {
            if (g == null)
            {
                player.guns[i] = GetComponent<Gun>(); 
                placed = true;
                break;
            }
            ++i;
        }
        if (!placed) player.guns.Add(GetComponent<Gun>());
    }

}
