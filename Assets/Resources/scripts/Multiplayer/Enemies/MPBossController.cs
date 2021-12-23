using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MPBossController : MPDirectioner
{
    void Awake()
    {
        def = FindObjectOfType<DefObj>();
        anim = GetComponent<EnemyAnimations>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy.coins += Random.Range(0, 11) * 10;
        iEnemy = GetComponent<MPBoss>();
        OnStart();
        if (PhotonNetwork.IsMasterClient)
        {
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

    public override void TakeDamage(float damage, Photon.Realtime.Player player)
    {
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
                    Drop();
                    MPEnemy.DestroyAfterTime(view, 30);
                    break;
                }
            }
        }
    }

}
