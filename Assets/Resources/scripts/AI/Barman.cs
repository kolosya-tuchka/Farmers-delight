using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barman : MonoBehaviour
{
    RoundManager manager;
    public GameObject barman;
    void Start()
    {
        manager = FindObjectOfType<RoundManager>();
    }

    void Update()
    {
        barman.SetActive(manager.isBreak);
    }
}
