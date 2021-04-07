using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notices : MonoBehaviour
{
    Text text;
    EnemyManager enemyManager;
    RoundManager roundManager;
    void Start()
    {
        text = GetComponent<Text>();
        enemyManager = GameObject.Find("Game Manager").GetComponent<EnemyManager>();
        roundManager = enemyManager.gameObject.GetComponent<RoundManager>();
    }

    void Update()
    {
        if (roundManager.isBreak)
        {
            text.text = "Round " + roundManager.round.ToString() + " is over!"
            + "Next round starts in " + Mathf.FloorToInt(roundManager.timeToNextRound).ToString() + " seconds"
            + "tap here to start next round now"; 
        }
        else if (enemyManager.enemiesOnSceneNow + enemyManager.allEnemiesNow <= 5) text.text = "Zombies left: " + (enemyManager.enemiesOnSceneNow + enemyManager.allEnemiesNow).ToString();
        else text.text = null;
    }
}
