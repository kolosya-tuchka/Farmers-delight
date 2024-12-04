using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingLobby : MonoBehaviourPunCallbacks
{
    public RoomInfoLobby RoomInfoPrefab;
    public SimplePool<RoomInfoLobby> RoomInfoPool;

    private List<RoomInfoLobby> RoomsInfo;

    private void Start()
    {
        RoomInfoPool.InitializePool(RoomInfoPrefab, transform, 10, true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var roomInfoLobby in RoomsInfo)
        {
            roomInfoLobby.OnInactive?.Invoke(roomInfoLobby);
        }
        RoomsInfo = new List<RoomInfoLobby>();

        foreach (var roomInfo in roomList)
        {
            var roomInfoLobby = RoomInfoPool.GetObject();
            roomInfoLobby.OnUpdate(roomInfo);
        }
    }
}
