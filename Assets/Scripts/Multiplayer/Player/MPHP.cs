using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MPHP : HP
{
    private PhotonView view;
    protected override void Start()
    {
        view = GetComponent<PhotonView>();
        base.Start();
    }
    
    protected override void Update()
    {
        RenderHP();
        if (!view.IsMine) return;
        base.Update();
    }
    
}
