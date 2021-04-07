using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    Player player;
    public Text rounds, coins;
    GameObject inGame, gameOver;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inGame = gameObject.transform.Find("InGame").gameObject;
        gameOver = gameObject.transform.Find("GameOver").gameObject;
        inGame.SetActive(true);
        gameOver.SetActive(false);

        GameObject.Find("Mobile").SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }

    void Update()
    {
        if (player.health.healPoints > 0)
        {
            rounds.text = GameObject.Find("Game Manager").GetComponent<RoundManager>().round.ToString();
            coins.text = player.coins.ToString();
        }
        else Invoke("GameOver", 1);
    }

    public void GameOver()
    {
        inGame.SetActive(false);
        gameOver.SetActive(true);
        //Destroy(player.gameObject);
    }
}
