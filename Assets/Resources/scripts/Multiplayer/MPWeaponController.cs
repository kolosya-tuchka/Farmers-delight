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

    [PunRPC]
    void Sync(int playerIndex)
    {
        mp = FindObjectOfType<MPManager>();
        var player = mp.players[playerIndex].GetComponent<Player>();
        int i = 0;
        bool placed = false;
        transform.parent = player.weaponsPrefab.transform;
        foreach (var g in player.weapons)
        {
            if (g == null)
            {
                player.weapons[i] = GetComponent<Gun>(); 
                placed = true;
                break;
            }
            ++i;
        }
        if (!placed) player.weapons.Add(GetComponent<Gun>());
        transform.localPosition = GetComponent<Gun>().localPos;
    }

}
