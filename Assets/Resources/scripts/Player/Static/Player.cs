using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed, reloadBoost, shootingBoost, speedBoost;
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

        health = GetComponent<HP>();
        anim = GetComponent<PlayerAnimations>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, focusDistance);
    }
}
