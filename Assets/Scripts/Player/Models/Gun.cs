using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public float reloadTime, reloadProgress, curAmmo, maxAmmo;
    public int magazines, maxMagazines;
    public bool isReloading;
    public bool canReload
    {
        get
        {
            return (Input.GetKeyDown(KeyCode.R) || curAmmo == 0) && !isReloading && curAmmo != maxAmmo;
        }
    }

    void Start()
    {
        magazines = maxMagazines;
        curAmmo = maxAmmo;
        reloadProgress = 0;
        isReloading = false;
        canAttack = true;
    }

}

public abstract class Weapon : MonoBehaviour
{
    public Player owner;
    public Vector3 localPos;
    public SpriteRenderer model;
    public string weaponName;
    public float attackRate;
    public bool canAttack;
    public bool isFocused = false;
    public MoveMode moveMode = MoveMode.rotate; 
    public enum MoveMode
    {
        rotate, flip
    }
}
