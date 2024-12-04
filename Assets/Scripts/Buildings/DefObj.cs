using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefObj : MonoBehaviour
{
    public HP hp;
    public List<Transform> destinationPoints; 

    void Update()
    {
        if (hp.healPoints <= 0)
        {
            FindObjectOfType<InterfaceManager>().GameOver();
            FindObjectOfType<PlayerController>().GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy") other.gameObject.GetComponent<Enemy>().canDestroy = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy") other.gameObject.GetComponent<Enemy>().canDestroy = false;
    }

    public Transform GetDestinationPoint()
    {
        return destinationPoints[Random.Range(0, destinationPoints.Count)];
    }
}
