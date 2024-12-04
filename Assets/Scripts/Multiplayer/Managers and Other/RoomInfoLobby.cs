using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoomInfoLobby : MonoBehaviour, IPoolableObject
{
    public UnityEvent<IPoolableObject> OnInactive { get; }
    
    private RoomInfo RoomInfo;
    public Text RoomNameText, PlayersText;

    public void OnUpdate(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        RoomNameText.text = RoomInfo.Name;
        PlayersText.text = $"{RoomInfo.PlayerCount}/{RoomInfo.MaxPlayers}";
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

}
