using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MPGunSale : GunSale
{
    PhotonView view;
    MPManager mp;
    string mpPath = "Multiplayer/Guns/";
    bool canBuy;
    void Start()
    {
        canBuy = false;
        mp = FindObjectOfType<MPManager>();
        player = mp.player;
        view = player.GetComponent<PhotonView>();

        sign = GetComponent<SaleSign>();
        image.sprite = sign.item.GetComponent<Gun>().model.sprite;
        text.text = sign.cost.ToString();
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        Check();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            text.gameObject.SetActive(true);
            canBuy = true;
        }
        player.GetComponent<Player>().target = gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            canBuy = false;
            text.gameObject.SetActive(false);
        }
    }

    protected override void Check()
    {
        if (!view.IsMine) return;

        base.Check();
       
    }

    [PunRPC]
    protected override void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = PhotonNetwork.Instantiate(mpPath + sign.item.name, player.transform.position, Quaternion.identity);
        gun.transform.parent = pl.weaponsPrefab.transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.gunCapacity == pl.weapons.Count)
        {
            PhotonNetwork.Destroy(pl.weapons[pl.currentGun].gameObject);
            pl.weapons.RemoveAt(pl.currentGun);
        }
        pl.weapons.Add(gun.GetComponent<Gun>());
        pl.currentGun = pl.weapons.Capacity - 2;
        gun.GetComponent<PhotonView>().RPC("Sync", RpcTarget.Others, mp.playerIndex);
        weaponUI.ImageUpdate();

        pl.coins -= sign.cost;
    }

}
