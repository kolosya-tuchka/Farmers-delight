using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPEnemyManager : EnemyManager
{
    string mpPath = "Multiplayer/Enemies/";

    public override void SpawnBoss()
    {
        var manager = GetComponent<BossManager>();
        manager.hpMultiplier *= 2;
        var mas = GameObject.FindGameObjectsWithTag("EnemySpawner");
        var boss = PhotonNetwork.Instantiate(mpPath + manager.boss.name, mas[UnityEngine.Random.Range(0, mas.Length)].transform.position, Quaternion.identity);
        boss.GetComponent<HP>().maxHP *= manager.hpMultiplier;
    }

    public override void SpawnEnemy()
    {
        var mas = GameObject.FindGameObjectsWithTag("EnemySpawner");
        if (GetComponent<RoundManager>().round > 5)
        {
            enemy = enemies[Random.Range(0, enemies.Length)];
        }
        var en = PhotonNetwork.Instantiate(mpPath + enemy.name, mas[UnityEngine.Random.Range(0, mas.Length)].transform.position, Quaternion.identity);
        enemiesOnSceneNow++;
        if (GetComponent<RoundManager>().roundType == RoundManager.RoundType.simple)
        {
            allEnemiesNow--;
        }
        en.transform.parent = gameObject.transform;
        en.GetComponent<Enemy>().hp.maxHP *= GetComponent<RoundManager>().hp;
    }
}
