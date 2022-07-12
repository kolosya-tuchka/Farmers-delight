using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MPEnemy : Enemy, IMPEnemy
{
    [PunRPC]
    public void Die(int playerIndex)
    {
        base.Die(MPManager.players[playerIndex].GetComponent<Player>());
        if (GetComponent<PhotonView>().IsMine)
        {
            StartCoroutine(DestroyAfterTime(GetComponent<PhotonView>(), 30));
        }
    }

    public static IEnumerator DestroyAfterTime(PhotonView obj, float time)
    {
        yield return new WaitForSeconds(time);

        PhotonNetwork.Destroy(obj);
    }
}
