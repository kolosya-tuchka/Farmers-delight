using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MPDirectioner : Directioner, IPunOwnershipCallbacks, IPunObservable
{
    [HideInInspector] public MPManager mp;
    [HideInInspector] public Player killer;

    void Start()
    {
        mp = FindObjectOfType<MPManager>();

        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent2D>();
        agent.speed = enemy.speed;
        agent.stoppingDistance = enemy.attackDistance;
        enemy.behaviour = Enemy.Behaviour.attacker;
        enemy.state = Enemy.State.alive;
        def = FindObjectOfType<DefObj>().gameObject;
        manager = FindObjectOfType<EnemyManager>();
        anim = GetComponent<EnemyAnimations>();

        if (enemy.behaviour == Enemy.Behaviour.attacker) maxTimeOfPersuit = 10;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Move());
        }

        if (CompareTag("Enemy"))
        {
            enemy.coins += Random.Range(-3, 6);
        }

    }

    void FixedUpdate()
    {
        obj = FindNearestPlayer()?.gameObject;

        if (PhotonNetwork.IsMasterClient && obj != null)
        {
            Check();
        }

        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead && killer != null)
        {
            int i = mp.playerIndex;
            GetComponent<PhotonView>().RPC("Die", RpcTarget.Others, i);
            enemy.Die(i);
        }
    }

    Transform FindNearestPlayer()
    {
        Transform playerTransform = null;
        float minDistance = Vector2.Distance(transform.position, mp.players[0].transform.position);
        foreach (var player in mp.players)
        {
            if (!player.activeInHierarchy || player == null) continue;

            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < minDistance || playerTransform == null)
            {
                playerTransform = player.transform;
                minDistance = distance;
            }
        }
        return playerTransform;
    }

    public override void Attack()
    {
        rend.flipX = transform.position.x > obj.transform.position.x;
        anim.isAttack = true;
        timeOfPersuit = 0;
        GetComponent<PhotonView>().RPC("MPAttack", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void MPAttack()
    {
        if (!obj.GetComponent<PhotonView>().IsMine) return;
        var player = mp.player.GetComponent<HP>();
        player.healPoints -= enemy.damage;
        player.delayTimeLeft = player.delayOfRegeneration;

        if (player.healPoints <= 0) player.GetComponent<Player>().isAlive = false; 
    }

    public override void Drop()
    {
        if (killer == null) return;
        base.Drop();
    }

    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {
        if (PhotonNetwork.IsMasterClient) StartCoroutine(Move());
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Photon.Realtime.Player senderOfFailedRequest)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rend.flipX);
            stream.SendNext(def.GetComponent<HP>().healPoints);
            stream.SendNext(anim.isAttack);
        }
        else
        {
            rend.flipX = (bool)stream.ReceiveNext();
            def.GetComponent<HP>().healPoints = (float)stream.ReceiveNext();
            anim.isAttack = (bool)stream.ReceiveNext();
        }
    }

}
