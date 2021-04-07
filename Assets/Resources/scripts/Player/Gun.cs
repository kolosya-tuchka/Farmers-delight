using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float reloadTime, reloadProgress, curAmmo, maxAmmo;
    public int magazines, maxMagazines;
    [HideInInspector] public float rt;
    public bool isReloading;
    public Vector3 localPos;
    public SpriteRenderer model;
    public string name;
    void Start()
    {
        magazines = maxMagazines;
        curAmmo = maxAmmo;
        reloadProgress = 0;
        isReloading = false;
    }

    void Update()
    {
        rt = reloadTime / GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().reloadBoost;
        if (reloadProgress == rt) reloadProgress = 0;
        if ( magazines > 0 && ((curAmmo == 0) || (Input.GetKeyDown(KeyCode.R) && !isReloading) || isReloading))
        {
            isReloading = true;
            reloadProgress += Time.deltaTime;
            if (Mathf.Abs(rt - reloadProgress) < Time.deltaTime)
            {
                reloadProgress = rt;
                isReloading = false;
                curAmmo = maxAmmo;
                magazines--;
            }
        }
    }
}
