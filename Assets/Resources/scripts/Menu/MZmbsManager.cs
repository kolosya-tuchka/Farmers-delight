using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MZmbsManager : MonoBehaviour
{
    public GameObject enemy;
    void Start()
    {
        InvokeRepeating("EnemySpawn", 0, 1);
    }
    public void EnemySpawn()
    {
         var mas = GameObject.FindGameObjectsWithTag("EnemySpawner");
         var en = Instantiate(enemy, mas[UnityEngine.Random.Range(0, mas.Length)].transform.position, Quaternion.identity);
         en.transform.parent = gameObject.transform;
    }
}
