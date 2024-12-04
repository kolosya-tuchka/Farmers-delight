using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] protected Player player;
    [SerializeField] protected Text rounds, coins;
    [SerializeField] protected GameObject inGame, menu, gameOver, mobile;
    [SerializeField] protected RoundManager rm;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Awake()
    {
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
            rounds.text = rm.round.ToString();
            coins.text = player.coins.ToString();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ActiveMenu();
            }
        }
        else Invoke("GameOver", 1);
    }
}
