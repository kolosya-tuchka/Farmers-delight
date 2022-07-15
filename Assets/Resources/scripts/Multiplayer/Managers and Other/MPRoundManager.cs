using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPRoundManager : RoundManager, IPunObservable
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
        if (mp.aliveCount <= 0) return;

        if (PhotonNetwork.IsMasterClient)
        {
            view.TransferOwnership(PhotonNetwork.MasterClient);

            if (round > 0)
            {
                if (!isBreak && manager.allEnemiesNow <= 0 && manager.enemiesOnSceneNow <= 0)
                {
                    isBreak = true;
                    RespawnPlayers();
                    view.RPC("RespawnPlayers", RpcTarget.Others);
                }
                if (Input.GetKeyDown(KeyCode.Space)) view.RPC("NextRound", RpcTarget.AllViaServer);
                if (timeToNextRound <= 0) view.RPC("NextRound", RpcTarget.AllViaServer);
            }

            object[] parameters = new object[4];
            parameters[0] = manager.allEnemiesNow;
            parameters[1] = manager.enemiesOnSceneNow;
            parameters[2] = round;
            parameters[3] = (int)roundType;
            view.RPC("Sync", RpcTarget.Others, parameters);
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
        base.NextRound();
    }

    public void StartGame()
    {
        int playerCount = MPManager.players.Count;
        manager.allEnemies = manager.allEnemies * (playerCount + 1) / 2;
        manager.enemiesOnScene = manager.enemiesOnScene * (playerCount + 1) / 2;
        StartCoroutine(CountDown());
        NextRound();
    }

    [PunRPC]
    void RespawnPlayers()
    {
        mp.aliveCount = MPManager.players.Count;
        foreach (var p in MPManager.players)
        {
            p.SetActive(true);
            p.GetComponent<Player>().isAlive = true;
            var hp = p.GetComponent<HP>();
            hp.healPoints = hp.maxHP;
        }
    }
    
    [PunRPC]
    public void Sync(object[] parameters)
    {
        manager.allEnemiesNow = (int)parameters[0];
        manager.enemiesOnSceneNow = (int)parameters[1];
        round = (int)parameters[2];
        roundType = (RoundType)parameters[3];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isBreak);   
        }
        else
        {
            isBreak = (bool)stream.ReceiveNext();
        }
    }
}
