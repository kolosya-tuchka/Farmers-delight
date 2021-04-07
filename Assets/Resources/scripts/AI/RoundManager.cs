using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int round;
    public float timeToNextRound;
    /*[HideInInspector]*/ public float hp;
    [HideInInspector] public bool isBreak;
    EnemyManager manager;
    public RoundType roundType;

    public enum RoundType
    {
        simple, boss
    }

    void Start()
    {
       manager = gameObject.GetComponent<EnemyManager>();
       hp = 1;
       timeToNextRound = 0;
       isBreak = false;
       StartCoroutine(CountDown());
       NextRound();
    }

    void Update()
    {
        if (!isBreak && manager.allEnemiesNow <= 0 && manager.enemiesOnSceneNow <= 0)
        {
            isBreak = true;
        }
        if (Input.GetKeyDown(KeyCode.Space)) NextRound();
        if (timeToNextRound <= 0) NextRound();
    }

    public void NextRound()
    {
        if (isBreak)
        {
            manager.enemiesOnSceneNow = 0;
            isBreak = false;
            manager.StopAllCoroutines();
            round++;
            manager.allEnemies += manager.allEnemies / 5;
            manager.enemiesOnScene += manager.enemiesOnScene / 5;
            hp *= 1.2f;
            manager.allEnemiesNow = manager.allEnemies;
            if (round % 5 == 0)
            {
                roundType = RoundType.boss;
                manager.StartCoroutine(manager.BossSpawn());
            }
            else
            {
                roundType = RoundType.simple;
                manager.StartCoroutine(manager.EnemySpawn());
            }
            timeToNextRound = 60;
        }
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            if (timeToNextRound > 0 && isBreak)
            {
                timeToNextRound--;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return null;
            }
        }
    }
}
