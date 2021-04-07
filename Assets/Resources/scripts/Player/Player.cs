using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed,reloadBoost, shootingBoost;
    public int kills, coins;
    public PlayerManager.Players title;
    public List<Gun> guns;
    public int currentGun, gunCapacity;
    public GameObject target;
    public HP health = new HP();
    public bool used;
    private void Start()
    {
        kills = 0;
        coins = 0;
        reloadBoost = 1;
        shootingBoost = 1;
    }
}
