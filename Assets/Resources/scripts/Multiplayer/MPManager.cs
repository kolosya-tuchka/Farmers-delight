using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class MPManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public int playerIndex;
    public GameObject gameManager, playerPrefab, player, HPBar;
    public List<GameObject> players;
    public bool isGameStarted;
    public GameObject startButton;
    public int aliveCount;
    Vector2 spawnPos;

    void Awake()
    {
        SpawnPlayer();
        isGameStarted = false;
        aliveCount = 1;

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
        camera.Follow = player.transform;

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
        gameManager.GetComponent<MPRoundManager>().StartGame();

        int i = 0;
        var _players = FindObjectsOfType<Player>();
        foreach (var p in _players)
        {
            if (p == player) playerIndex = i;
            players.Add(p.gameObject);
            ++i;
        }
        aliveCount = players.Count;
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
