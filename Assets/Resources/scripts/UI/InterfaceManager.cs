using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    Player player;
    public Text rounds, coins;
    public GameObject inGame, menu, gameOver;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inGame.SetActive(true);
        menu.SetActive(false);

        GameObject.Find("Mobile").SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }

    void Update()
    {
        if (player.health.healPoints > 0)
        {
            rounds.text = GameObject.Find("Game Manager").GetComponent<RoundManager>().round.ToString();
            coins.text = player.coins.ToString();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ActiveMenu();
            }
        }
        else Invoke("GameOver", 1);
    }

    public void GameOver()
    {
        inGame.SetActive(false);
        gameOver.SetActive(true);
    }
    public void ActiveMenu()
    {
        inGame.SetActive(!inGame.activeInHierarchy);
        menu.SetActive(!menu.activeInHierarchy);
        Time.timeScale = menu.activeInHierarchy ? 0 : 1;
    }
}
