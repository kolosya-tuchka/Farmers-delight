using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int allEnemiesNow = 0, allEnemies = 20, enemiesOnScene = 4, enemiesOnSceneNow = 0;
    public GameObject enemy;
    public GameObject[] enemies;
    System.Func<bool> spawnPredicate;

    public IEnumerator EnemySpawn()
    {
        while (!GetComponent<RoundManager>().isBreak)
        {
            if (enemiesOnSceneNow < enemiesOnScene && allEnemiesNow > 0)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator BossSpawn()
    {
        spawnPredicate = predicate;
        SpawnBoss();
        
        while (!GetComponent<RoundManager>().isBreak)
        {
            yield return new WaitUntil(spawnPredicate);
            SpawnEnemy();
            yield return null;
        }
    }

    public virtual void SpawnEnemy()
    {
        var mas = GameObject.FindGameObjectsWithTag("EnemySpawner");
        if (GetComponent<RoundManager>().round > 5)
        {
            enemy = enemies[Random.Range(0, enemies.Length)];
        }
        var en = Instantiate(enemy, mas[UnityEngine.Random.Range(0, mas.Length)].transform.position, Quaternion.identity);
        enemiesOnSceneNow++;
        if (GetComponent<RoundManager>().roundType == RoundManager.RoundType.simple)
        {
            allEnemiesNow--;
        }
        en.transform.parent = gameObject.transform;
        en.GetComponent<Enemy>().hp.maxHP *= GetComponent<RoundManager>().hp;
    }

    public virtual void SpawnBoss()
    {
        var manager = GetComponent<BossManager>();
        manager.hpMultiplier *= 2;
        var mas = GameObject.FindGameObjectsWithTag("EnemySpawner");
        var boss = Instantiate(manager.boss, mas[UnityEngine.Random.Range(0, mas.Length)].transform.position, Quaternion.identity);
        boss.GetComponent<HP>().maxHP *= manager.hpMultiplier;
    }

    bool predicate()
    {
        return enemiesOnSceneNow < 5;
    }
}
