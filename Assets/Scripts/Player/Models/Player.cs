using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float curSpeed, speed, reloadBoost, shootingBoost, speedBoost;
    public int kills, coins;
    public List<Weapon> weapons;
    public int currentGun, gunCapacity;
    public GameObject target, weaponsPrefab;
    public HP health;
    public SpriteRenderer rend;
    public PlayerAnimations anim;
    public bool used, isAlive;
    public float focusDistance;

    private void Awake()
    {
        kills = 0;
        coins = 0;
        reloadBoost = 1;
        shootingBoost = 1;
        speedBoost = 1;
        isAlive = true;

        ResetSpeed();
        
        health = GetComponent<HP>();
        anim = GetComponent<PlayerAnimations>();
    }

    public void ResetSpeed()
    {
        curSpeed = speed;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, focusDistance);
    }
}
