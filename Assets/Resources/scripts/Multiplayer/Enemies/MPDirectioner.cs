using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPDirectioner : Directioner, IPunOwnershipCallbacks, IPunObservable
{
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
            StartCoroutine(Move());
        }
        enemy.coins += Random.Range(-3, 6);
        iEnemy = GetComponent<MPEnemy>();
    }

    public override void OnStart()
    {
        enemy = GetComponent<MPEnemy>();
        mp = FindObjectOfType<MPManager>();
        view = GetComponent<PhotonView>();
        base.OnStart();
    }

    void FixedUpdate()
    {
        player = FindNearestPlayer()?.GetComponent<Player>();

        if (PhotonNetwork.IsMasterClient && player != null)
        {
            Check();
        }
    }

    public virtual void TakeDamage(float damage, Photon.Realtime.Player player)
    {
        enemy.behaviour = Enemy.Behaviour.attacker;
        enemy.hp.healPoints -= damage;
        anim.hit = true;

        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead)
        {
            foreach (var p in mp.players)
            {
                if (p.GetComponent<PhotonView>().Owner == player)
                {
                    killer = mp.player.GetComponent<Player>();
                    iEnemy.Die(killer);
                    view.RPC("Die", RpcTarget.Others, mp.playerIndex);
                    var rounds = manager.GetComponent<RoundManager>();
                    if (rounds.roundType == RoundManager.RoundType.simple)
                    {
                        Drop();
                    }
                    MPEnemy.DestroyAfterTime(view, 30);
                    Destroy(this);
                    break;
                }
            }
        }
    }

    public Transform FindNearestPlayer()
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
        rend.flipX = transform.position.x > player.transform.position.x;
        anim.isAttack = true;
        timeOfPersuit = 0;
        GetComponent<PhotonView>().RPC("MPAttack", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void MPAttack()
    {
        if (!player.GetComponent<PhotonView>().IsMine) return;
        mp.player.GetComponent<MPPlayerController>().TakeDamage(enemy.damage);
    }

    public override void Drop()
    {
        if (killer == null) return;
        base.Drop();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rend.flipX);
            stream.SendNext(def.GetComponent<HP>().healPoints);
            stream.SendNext(anim.isAttack);
        }
        else if (stream.IsReading)
        {
            rend.flipX = (bool)stream.ReceiveNext();
            def.GetComponent<HP>().healPoints = (float)stream.ReceiveNext();
            anim.isAttack = (bool)stream.ReceiveNext();
        }
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
