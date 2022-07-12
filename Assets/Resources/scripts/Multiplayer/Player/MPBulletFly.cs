using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPBulletFly : BulletFly
{
    Enemy enemy;

    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        bullet = gameObject.GetComponent<Bullet>();

        rigidbody.velocity = transform.right * speed;
        StartCoroutine(MPEnemy.DestroyAfterTime(bullet.GetComponent<PhotonView>(), bullet.timeOfFlying));
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.CompareTag("Construction") || collision.gameObject.CompareTag("DefObj"))
        {
            Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            var view = GetComponent<PhotonView>();
            var dir = collision.GetComponent<MPDirectioner>();
            
            Instantiate(particle, gameObject.transform.position, gameObject.transform.rotation);
            if (view.IsMine)
            {
                dir.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)bullet.damage, MPManager.PlayerIndex(GetComponent<PhotonView>().Owner));
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

}
