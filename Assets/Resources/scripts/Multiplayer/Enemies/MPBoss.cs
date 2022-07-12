using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MPBoss : Boss, IMPEnemy
{
    [PunRPC]
    public void Die(int playerIndex)
    {
        base.Die(MPManager.players[playerIndex].GetComponent<Player>());
        if (GetComponent<PhotonView>().IsMine)
        {
            StartCoroutine(MPEnemy.DestroyAfterTime(GetComponent<PhotonView>(), 30));
        }
    }
}
