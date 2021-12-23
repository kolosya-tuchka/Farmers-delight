using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPGunController : GunController
{
    MPManager mp;
    void Awake()
    {
        gun = GetComponent<Gun>();
    }

    private void Start()
    {
        mp = FindObjectOfType<MPManager>();
        if (gun.owner != mp.player.GetComponent<Player>()) Destroy(this);
    }

    private void OnEnable()
    {
        StartCoroutine(ReloadCheck());
        StartCoroutine(DelayCheck());
    }

    private void OnDisable()
    {
        gun.isReloading = false;
    }
}
