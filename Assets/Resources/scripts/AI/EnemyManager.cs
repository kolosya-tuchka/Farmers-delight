using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int allEnemiesNow = 0, allEnemies = 20, enemiesOnScene = 4, enemiesOnSceneNow = 0;
    public GameObject enemy;
    public GameObject[] enemies;
    System.Func<bool> spawnPredicate;

    BossManager bossManager;
    RoundManager roundManager;

    GameObject[] spawners;

    private void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        bossManager = GetComponent<BossManager>();
        roundManager = GetComponent<RoundManager>();
    }

    public IEnumerator EnemySpawn()
    {
        while (!roundManager.isBreak)
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
        bossManager.curBossCount = bossManager.bossCount;
        spawnPredicate = predicate;
        for (int i = 0; i < bossManager.bossCount; ++i)
            SpawnBoss();
        StartCoroutine(EnemySpawnBoss());

        yield return new WaitUntil(() => bossManager.curBossCount == 0);
        foreach (var enemy in FindObjectsOfType<Directioner>())
        {
            enemy.TakeDamage(int.MaxValue);
        }
        roundManager.isBreak = true;
        ++bossManager.bossCount;
        bossManager.hpMultiplier *= 2;
        bossManager.coinsMultiplier *= 2;
    }

    IEnumerator EnemySpawnBoss()
    {
        while (true)
        {
            yield return new WaitUntil(spawnPredicate);
            if (bossManager.curBossCount == 0) yield break;
            SpawnEnemy();
        }
    }

    public virtual void SpawnEnemy()
    {
        if (GetComponent<RoundManager>().round > 5)
        {
            enemy = enemies[Random.Range(0, enemies.Length)];
        }
        var en = Instantiate(enemy, spawners[UnityEngine.Random.Range(0, spawners.Length)].transform.position, Quaternion.identity);
        enemiesOnSceneNow++;
        if (GetComponent<RoundManager>().roundType == RoundManager.RoundType.simple)
        {
            allEnemiesNow--;
        }
        en.transform.parent = gameObject.transform;
        en.GetComponent<Enemy>().hp.maxHP *= roundManager.hp;
    }

    public virtual void SpawnBoss()
    {
        var boss = Instantiate(bossManager.boss, spawners[UnityEngine.Random.Range(0, spawners.Length)].transform.position, Quaternion.identity);

        boss.GetComponent<HP>().maxHP *= bossManager.hpMultiplier;
        boss.GetComponent<Enemy>().coins *= (int)bossManager.coinsMultiplier;
    }

    bool predicate()
    {
        return enemiesOnSceneNow < 4 + bossManager.bossCount;
    }
}
