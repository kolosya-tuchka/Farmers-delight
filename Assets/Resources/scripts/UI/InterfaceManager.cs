using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public Player player;
    public Text rounds, coins;
    public GameObject inGame, menu, gameOver, mobile;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        inGame.SetActive(true);
        menu.SetActive(false);

        mobile.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }

    void Update()
    {
        Check();
    }

    public virtual void GameOver()
    {
        inGame.SetActive(false);
        gameOver.SetActive(true);
    }
    public virtual void ActiveMenu()
    {
        inGame.SetActive(!inGame.activeInHierarchy);
        menu.SetActive(!menu.activeInHierarchy);
        Time.timeScale = menu.activeInHierarchy ? 0 : 1;
    }

    public virtual void Check()
    {
        if (player.health.healPoints >= 0)
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
}
