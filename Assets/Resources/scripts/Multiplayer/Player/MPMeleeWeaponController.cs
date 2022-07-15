using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPMeleeWeaponController : MeleeWeaponController
{
    MPManager mp;
    void Awake()
    {
        weapon = GetComponent<MeleeWeapon>();
    }

    private void Start()
    {
        mp = FindObjectOfType<MPManager>();
        if (weapon.owner != mp.player.GetComponent<Player>()) Destroy(this);

        curSpeed = weapon.owner.curSpeed;
        weapon.owner.GetComponent<MPPlayerController>().GunSwap();
    }

    private void OnEnable()
    {
        StartCoroutine(DelayCheck());
    }

    private void OnDisable()
    {
        weapon.owner.curSpeed = curSpeed;
    }

    [PunRPC]
    public void Sync(int playerIndex, int weaponIndex)
    {
        var player = MPManager.players[playerIndex].GetComponent<Player>();

        if (player.gunCapacity == player.weapons.Count)
        {
            if (weaponIndex == 0) player.weapons[weaponIndex].gameObject.SetActive(false);
            player.weapons[weaponIndex] = weapon;
        }
        else player.weapons.Add(weapon);

        transform.parent = player.weaponsPrefab.transform;
        transform.localPosition = weapon.localPos;
    }
}
