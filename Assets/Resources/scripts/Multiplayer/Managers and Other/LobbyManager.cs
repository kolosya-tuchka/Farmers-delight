using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Button mpButton, join, create;
    public InputField inputNick;

    bool isNickValid
    {
        get
        {
            return inputNick.text.Length >= 3;
        }
    }

    void Start()
    {
        PhotonNetwork.NickName = "Slave №" + Random.Range(1000, 9999).ToString();

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "Alpha 1.3";
        PhotonNetwork.ConnectUsingSettings();

        CheckButtons();
    }

    public void OnValueChanged()
    {
        PhotonNetwork.NickName = inputNick.text;
        CheckButtons();
    }

    void CheckButtons()
    {
        join.interactable = isNickValid;
        create.interactable = isNickValid;
    }

    public override void OnConnectedToMaster()
    {
        mpButton.interactable = true;
        base.OnConnectedToMaster();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("Multiplayer");
    }

}
