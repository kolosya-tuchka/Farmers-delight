using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject boss;
    public float hpMultiplier, coinsMultiplier;
    public int bossCount, curBossCount;
    void Start()
    {
        hpMultiplier = coinsMultiplier = bossCount = curBossCount = 1;
    }
}
