using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barman : MonoBehaviour
{
    RoundManager manager;
    [SerializeField] GameObject barman;
    void Start()
    {
        manager = FindObjectOfType<RoundManager>();
    }

    void Update()
    {
        if (manager.isBreak != barman.activeInHierarchy)
        {
            barman.SetActive(manager.isBreak);
        }
    }
}
