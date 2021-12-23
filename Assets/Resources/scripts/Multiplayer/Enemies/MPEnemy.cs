using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MPEnemy : Enemy
{
    public override void Die(Player player)
    {
        base.Die(player);
    }

    public static IEnumerator DestroyAfterTime(PhotonView obj, float time)
    {
        yield return new WaitForSeconds(time);

        PhotonNetwork.Destroy(obj);
    }
}
