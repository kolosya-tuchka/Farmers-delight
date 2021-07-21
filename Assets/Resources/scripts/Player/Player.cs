using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed,reloadBoost, shootingBoost;
    public int kills, coins;
    public List<Gun> guns;
    public int currentGun, gunCapacity;
    public GameObject target, weapons;
    public HP health = new HP();
    public bool used, isAlive;
    private void Start()
    {
        kills = 0;
        coins = 1000;
        reloadBoost = 1;
        shootingBoost = 1;
        isAlive = true;
    }
}
