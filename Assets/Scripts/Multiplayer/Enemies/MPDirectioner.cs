using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

interface IMPDamage
{
    void TakeDamage(float damage, int playerIndex);
}

public interface IMPEnemy
{
    void Die(int playerIndex);
}

public class MPDirectioner : Directioner, IPunOwnershipCallbacks, IPunObservable, IMPDamage
{
    protected IMPEnemy iEnemy;
    protected MPManager mp;
    protected Player killer;
    protected PhotonView view;

    void Awake()
    {
        def = FindObjectOfType<DefObj>();
        anim = GetComponent<EnemyAnimations>();
    }

    void Start()
    {
        OnStart();
        if (enemy.behaviour == Enemy.Behaviour.attacker) maxTimeOfPersuit = 10;

        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("SyncOnStart", RpcTarget.Others, GetComponent<HP>().maxHP);
            StartCoroutine(Move());
        }
        enemy.coins += Random.Range(-3, 6);
        iEnemy = GetComponent<MPEnemy>();
    }

    public override void OnStart()
    {
        enemy = GetComponent<Enemy>();
        mp = FindObjectOfType<MPManager>();
        view = GetComponent<PhotonView>();
        base.OnStart();
        if (!PhotonNetwork.IsMasterClient)
        {
            agent.enabled = false;
        }
    }

    void FixedUpdate()
    {
        player = FindNearestPlayer()?.GetComponent<Player>();

        if (PhotonNetwork.IsMasterClient && player != null)
        {
            Check();
        }
    }

    [PunRPC]
    public virtual void TakeDamage(float damage, int playerIndex)
    {
        var player = MPManager.players[playerIndex].GetComponent<PhotonView>();
        enemy.behaviour = Enemy.Behaviour.attacker;
        enemy.hp.healPoints -= damage;
        anim.hit = true;

        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead)
        {
            if (player.IsMine)
            {
                killer = MPManager.players[playerIndex].GetComponent<Player>();
                iEnemy.Die(playerIndex);
                view.RPC("Die", RpcTarget.Others, playerIndex);
                var rounds = manager.GetComponent<RoundManager>();
                if (rounds.roundType == RoundManager.RoundType.simple)
                {
                    Drop();
                }
            }
        }
    }

    public Transform FindNearestPlayer()
    {
        Transform playerTransform = null;
        float minDistance = Vector2.Distance(transform.position, MPManager.players[0].transform.position);
        foreach (var player in MPManager.players)
        {
            if (player == null || !player.activeInHierarchy) continue;

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
        rend.flipX = transform.position.x > player.transform.position.x;
        anim.isAttack = true;
        timeOfPersuit = 0;
        GetComponent<PhotonView>().RPC("MPAttack", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void MPAttack()
    {
        if (player && !player.GetComponent<PhotonView>().IsMine) return;
        mp.player.GetComponent<MPPlayerController>().TakeDamage(enemy.damage);
    }

    public override void Drop()
    {
        if (killer == null) return;
        base.Drop();
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rend.flipX);
            stream.SendNext(def.hp.healPoints);
            stream.SendNext(anim.isAttack);
        }
        else if (stream.IsReading)
        {
            rend.flipX = (bool)stream.ReceiveNext();
            def.hp.healPoints = (float)stream.ReceiveNext();
            anim.isAttack = (bool)stream.ReceiveNext();
        }
    }
    
    [PunRPC]
    public virtual void SyncOnStart(float maxHP)
    {
        var hp = GetComponent<HP>();
        hp.maxHP = hp.healPoints = maxHP;
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
}
