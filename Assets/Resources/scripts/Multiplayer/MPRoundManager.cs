using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPRoundManager : RoundManager
{
    PhotonView view;
    MPManager mp;

    void Start()
    {
        manager = gameObject.GetComponent<EnemyManager>();
        hp = 1;
        timeToNextRound = 0;
        isBreak = true;
        mp = FindObjectOfType<MPManager>();
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            view.TransferOwnership(PhotonNetwork.MasterClient);

            if (round > 0)
            {
                if (!isBreak && manager.allEnemiesNow <= 0 && manager.enemiesOnSceneNow <= 0)
                {
                    isBreak = true;
                }
                if (Input.GetKeyDown(KeyCode.Space)) view.RPC("NextRound", RpcTarget.AllViaServer);
                if (timeToNextRound <= 0) view.RPC("NextRound", RpcTarget.AllViaServer);
            }

            object[] parametrs = new object[3];
            parametrs[0] = manager.allEnemiesNow;
            parametrs[1] = manager.enemiesOnSceneNow;
            parametrs[2] = round;
            view.RPC("Sync", RpcTarget.Others, parametrs);
        }
    }

    public override void ChooseRoundType()
    {
        if (!PhotonNetwork.IsMasterClient) return; 
        base.ChooseRoundType();
    }

    [PunRPC]
    public override void NextRound()
    {
        mp.aliveCount = mp.players.Count;
        foreach (var p in mp.players)
        {
            p.SetActive(true);
            var hp = p.GetComponent<HP>();
            hp.healPoints = hp.maxHP;
        }
        base.NextRound();
    }

    public void StartGame()
    {
        StartCoroutine(CountDown());
        NextRound();
    }

    [PunRPC]
    public void Sync(object[] parametrs)
    {
        manager.allEnemiesNow = (int)parametrs[0];
        manager.enemiesOnSceneNow = (int)parametrs[1];
        round = (int)parametrs[2];
    }
}
