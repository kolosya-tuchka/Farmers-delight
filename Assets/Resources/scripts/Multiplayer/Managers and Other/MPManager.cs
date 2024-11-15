﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;

public class MPManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static int playerIndex;
    public GameObject gameManager, playerPrefab, player, HPBar;
    public static List<GameObject> players;
    public bool isGameStarted;
    public GameObject startButton;
    public int aliveCount;
    Vector2 spawnPos;

    [SerializeField] private PlayerListUI _playerListUI;

    void Awake()
    {
        SpawnPlayer();
        isGameStarted = false;
        aliveCount = 1;

        players = new List<GameObject>();
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        player.GetComponent<PhotonView>().RPC("SyncOnStart", RpcTarget.Others, player.name);

        foreach (var model in FindObjectsOfType<SkinSwapper>())
        {
            model.SyncSkins();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (isGameStarted)
        {
            int i = 0;
            while (players[i].GetComponent<PhotonView>().Owner != otherPlayer) ++i;
            players.RemoveAt(i);
        }
    }

    void SpawnPlayer()
    {
        spawnPos = new Vector2(Random.Range(0, 6), Random.Range(8, 11));
        player = PhotonNetwork.Instantiate("Multiplayer/"+playerPrefab.name, spawnPos, Quaternion.identity);
        player.transform.parent = transform;

        var camera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        camera.Follow = player.GetComponentInChildren<CameraMove>().transform;

        player.GetComponent<HP>().bar = HPBar.GetComponent<Image>();
    }

    public void StartGame()
    {
        isGameStarted = true;

        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(0, null, options, sendOptions);
            PhotonNetwork.CurrentRoom.IsOpen = false;

            startButton.SetActive(false);
        }
        
        var _players = FindObjectsOfType<Player>();
        foreach (var p in _players)
        {
            players.Add(p.gameObject);
        }
        players.Sort(delegate(GameObject cur, GameObject next)
        {
            return cur.GetComponent<PhotonView>().Owner.ActorNumber.CompareTo(next.GetComponent<PhotonView>().Owner.ActorNumber);
        });
        foreach (var p in players)
        {
            if (p.GetComponent<PhotonView>().IsMine)
            {
                playerIndex = players.IndexOf(p);
                break;
            }
        }
        aliveCount = players.Count;
        gameManager.GetComponent<MPRoundManager>().StartGame();
        _playerListUI.UpdatePlayerCards();
    }

    public static int PlayerIndex(Player player)
    {
        return players.IndexOf(player.gameObject);
    }

    public static int PlayerIndex(Photon.Realtime.Player player)
    {
        return players.IndexOf(players.FirstOrDefault((p) => p.GetComponent<PhotonView>().Owner == player));
    }
    
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 0:
                StartGame();
                break;
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (!isGameStarted) startButton.SetActive(PhotonNetwork.IsMasterClient);
        else Leave();
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
