using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barman : MonoBehaviour
{
    RoundManager manager;
    void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RoundManager>();
    }

    void Update()
    {
        gameObject.transform.Find("Barman").gameObject.SetActive(manager.isBreak);
    }
}
