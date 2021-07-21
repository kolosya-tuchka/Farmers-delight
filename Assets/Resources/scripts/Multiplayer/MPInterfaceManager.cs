using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPInterfaceManager : InterfaceManager
{
    public GameObject deathWindow;
    MPManager mp;
    PhotonView view;

    void Start()
    {
        mp = FindObjectOfType<MPManager>();
        player = mp.player.GetComponent<Player>();
        inGame.SetActive(true);
        menu.SetActive(false);
        deathWindow.SetActive(false);
        view = player.GetComponent<PhotonView>();

        mobile.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }

    void Update()
    {
        if (!player.gameObject.activeInHierarchy)
        {
            if (mp.aliveCount > 0 && !deathWindow.activeInHierarchy) StartCoroutine(ShowAndHideDeathWindow());
            else if (mp.aliveCount <= 0) Invoke("GameOver", 1);
        }

        else if (player.health.healPoints >= 0)
        {
            if (view.IsMine)
            {
                rounds.text = GameObject.Find("Game Manager").GetComponent<RoundManager>().round.ToString();
                coins.text = player.coins.ToString();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ActiveMenu();
                }
            }
        }
    }

    public override void ActiveMenu()
    {
        inGame.SetActive(!inGame.activeInHierarchy);
        menu.SetActive(!menu.activeInHierarchy);
    }

    public override void GameOver()
    {
        if (mp.aliveCount > 0) return;
        StopAllCoroutines();
        deathWindow.SetActive(false);
        base.GameOver();
    }

    IEnumerator ShowAndHideDeathWindow()
    {
        inGame.SetActive(false);
        deathWindow.SetActive(true);

        yield return null;
        yield return new WaitUntil(() => player.gameObject.activeInHierarchy);

        inGame.SetActive(true);
        deathWindow.SetActive(false);
    }
}
