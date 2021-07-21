using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPPlayerController : PlayerController, IPunObservable
{
    PhotonView view;
    MPManager mp;
    bool flipX;

    void Start()
    {
        view = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        mp = FindObjectOfType<MPManager>();

        player.currentGun = 0;
        //var gunz = GameObject.FindGameObjectsWithTag("Gun");
        //foreach (var gun in gunz)
        //{
        //    if (gun.transform.parent.gameObject.transform.parent.gameObject == gameObject) player.guns.Add(gun.GetComponent<Gun>());
        //}
        GunSwap();

        GetComponent<PlayerAnimations>().enabled = view.IsMine;

        if (view.IsMine) view.RPC("SyncOnStart", RpcTarget.Others, gameObject.name);
    }

    void Update()
    {
        CheckMove();
        if (player.health.healPoints <= 0) GameOver();
        if (Input.GetKeyDown(KeyCode.T) && view.IsMine) view.RPC("GunSwap", RpcTarget.AllViaServer);
        renderer.flipX = flipX;
    }

    public override void CheckMove()
    {
        if (!view.IsMine) return;
        base.CheckMove();
        flipX = renderer.flipX;
    }

    [PunRPC]
    public override void GunSwap()
    {
        base.GunSwap();
    }

    public override void GameOver()
    {
        mp.aliveCount--;
        gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(flipX);
            stream.SendNext(player.health.healPoints);
        }
        else
        {
            flipX = (bool)stream.ReceiveNext();
            player.health.healPoints = (float)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void SyncOnStart(string nick)
    {
        gameObject.name = nick;
        gameObject.transform.parent = FindObjectOfType<MPManager>().transform;
    }

}
