using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPGunController : GunController
{
    MPManager mp;
    void Awake()
    {
        gun = GetComponent<Gun>();
    }

    private void Start()
    {
        if (!GetComponent<PhotonView>().IsMine) return;
        gun.owner.GetComponent<MPPlayerController>().GunSwap();
        mp = FindObjectOfType<MPManager>();
    }

    private void OnEnable()
    {
        if (!GetComponent<PhotonView>().IsMine) return;
        StartCoroutine(ReloadCheck());
        StartCoroutine(DelayCheck());
    }

    private void OnDisable()
    {
        gun.owner.ResetSpeed();
        gun.isReloading = false;
    }

    [PunRPC]
    public void Sync(int playerIndex, int weaponIndex)
    {
        var player = MPManager.players[playerIndex].GetComponent<Player>();

        if (player.gunCapacity == player.weapons.Count)
        {
            if (weaponIndex == 0) player.weapons[weaponIndex].gameObject.SetActive(false);
            player.weapons[weaponIndex] = gun;
        }
        else player.weapons.Add(gun);
        
        transform.parent = player.weaponsPrefab.transform;
        transform.localPosition = gun.localPos;
    }
}
