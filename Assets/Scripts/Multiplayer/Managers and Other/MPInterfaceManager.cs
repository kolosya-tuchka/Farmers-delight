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

        else if (mp.aliveCount > 0)
        {
            if (view.IsMine)
            {
                rounds.text = rm.round.ToString();
                coins.text = player.coins.ToString();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ActiveMenu();
                }
            }
        }
    }

    public void StartUse()
    {
        var pc = player.GetComponent<PlayerController>();
        pc.StartUse();
    }

    public void SwapGun()
    {
        var pc = player.GetComponent<PlayerController>();
        pc.GunSwap();
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
        base.GameOver();
    }

    IEnumerator ShowAndHideDeathWindow()
    {
        deathWindow.SetActive(true);

        yield return null;
        yield return new WaitUntil(() => player.gameObject.activeInHierarchy);

        deathWindow.SetActive(false);
    }
}
