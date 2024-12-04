using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MPBossController : MPDirectioner, IMPDamage, IPunObservable
{
    void Awake()
    {
        def = FindObjectOfType<DefObj>();
        anim = GetComponent<EnemyAnimations>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy.coins += Random.Range(0, 11) * 10;
        iEnemy = GetComponent<MPBoss>();
        OnStart();
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("SyncOnStart", RpcTarget.Others, GetComponent<HP>().maxHP);
            StartCoroutine(Move());
        }
    }

    void FixedUpdate()
    {
        player = FindNearestPlayer()?.GetComponent<Player>();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(repRate);
            repRate = enemy.repRate;
            PathToPlayer();
        }
    }

    [PunRPC]
    public override void TakeDamage(float damage, int playerIndex)
    {
        var player = MPManager.players[playerIndex].GetComponent<PhotonView>();
        enemy.hp.healPoints -= damage;
        anim.hit = true;

        if (enemy.hp.healPoints <= 0 && enemy.state != Enemy.State.dead)
        {
            foreach (var p in MPManager.players)
            {
                if (player.IsMine)
                {
                    killer = mp.player.GetComponent<Player>();
                    iEnemy.Die(playerIndex);
                    view.RPC("Die", RpcTarget.Others, playerIndex);
                    Drop();
                    view.RPC("Drop", RpcTarget.Others);
                    break;
                }
            }
        }
    }

    [PunRPC]
    public override void Drop()
    {
        base.Drop();
    }

    [PunRPC]
    public override void SyncOnStart(float maxHP)
    {
        var hp = GetComponent<HP>();
        hp.maxHP = hp.healPoints = maxHP;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rend.flipX);
            stream.SendNext(anim.isAttack);
        }
        else if (stream.IsReading)
        {
            rend.flipX = (bool)stream.ReceiveNext();
            anim.isAttack = (bool)stream.ReceiveNext();
        }
    }
}
