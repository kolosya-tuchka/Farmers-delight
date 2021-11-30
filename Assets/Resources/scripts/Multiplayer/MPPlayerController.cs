using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MPPlayerController : PlayerController, IPunObservable 
{
    PhotonView view;
    MPManager mp;

    void Awake()
    {
        view = GetComponent<PhotonView>();

        player = GetComponent<Player>();
        mp = FindObjectOfType<MPManager>();
        player.currentGun = 0;

        gameObject.name = PhotonNetwork.NickName;
        GunSwap();

        GetComponent<PlayerAnimations>().enabled = view.IsMine;

        if (view.IsMine)
        {
            view.RPC("SyncOnStart", RpcTarget.AllViaServer, gameObject.name);
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                gameObject.AddComponent<PlayerMoves>();
                gameObject.AddComponent<WeaponController>();
            }
            else if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                var mobile = FindObjectOfType<MobileUI>();
                var moves = gameObject.AddComponent<PlayerMovesMobile>();
                var weaponControls = gameObject.AddComponent<WeaponControllerMobile>();

                moves.joystick = mobile.joystick.GetComponent<Joystick>();
            }
        }
        else
        {
            player.health.canRegenerate = false;
        }
    }

    void Update()
    {
        if (!view.IsMine) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            GunSwap();
        }
    }

    [PunRPC]
    public override void GameOver()
    {
        mp.aliveCount--;
        view.RPC("GameOver", RpcTarget.Others);
        gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(player.rend.flipX);
            stream.SendNext(player.health.healPoints);
            foreach (var g in player.weapons)
            {
                stream.SendNext(g.gameObject.activeInHierarchy);
            }
        }
        else
        {
            player.rend.flipX = (bool)stream.ReceiveNext();
            player.health.healPoints = (float)stream.ReceiveNext();
            foreach (var g in player.weapons)
            {
                g.gameObject.SetActive((bool)stream.ReceiveNext());
            }
        }
    }

    [PunRPC]
    void SyncOnStart(string nick)
    {
        gameObject.name = nick;
        gameObject.transform.parent = mp.transform;
    }
    
}
