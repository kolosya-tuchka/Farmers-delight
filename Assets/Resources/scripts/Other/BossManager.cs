using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject boss;
    public float hpMultiplier;
    void Start()
    {
        hpMultiplier = 1;
    }
}
