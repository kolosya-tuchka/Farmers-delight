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
            return PhotonNetwork.NickName.Length >= 4;
        }
    }

    void Start()
    {
        string nick = PlayerPrefs.GetString("nickname");
        if (nick == null)
        {
            PhotonNetwork.NickName = "Slave №" + Random.Range(1000, 9999).ToString();
        }
        else
        {
            PhotonNetwork.NickName = nick;
        }
        inputNick.text = PhotonNetwork.NickName;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "Beta 1.2";
        PhotonNetwork.ConnectUsingSettings();
        CheckButtons();
    }

    public void OnValueChanged()
    {
        PhotonNetwork.NickName = inputNick.text;
        CheckButtons();
    }

    public void OnEndEdit()
    {
        PlayerPrefs.SetString("nickname", inputNick.text);
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
