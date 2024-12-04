using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkinSwapper : MonoBehaviour
{
    public PlayerSkins skins;
    private PhotonView view;
    private Animator animator;
    private int skin;
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view == null || !view.IsMine) return;
        skin = PlayerPrefs.GetInt("PlayerSkin");
        animator = GetComponent<Animator>();
        SwapSkin(skin);
        SyncSkins();
    }

    public void SyncSkins()
    {
        if (view == null || !view.IsMine) return;
        view.RPC("SwapSkin", RpcTarget.AllViaServer, skin);
    }
    
    [PunRPC]
    void SwapSkin(int skin)
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = skins.skins[skin];
    }
}
