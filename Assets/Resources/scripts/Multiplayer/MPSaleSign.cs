using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MPSaleSign : SaleSign
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

        sale = GetComponent<GunSale>();
        image.sprite = sale.gun.GetComponent<Gun>().model.sprite;
        text.text = sale.cost.ToString();
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

    public override void Check()
    {
        if (!view.IsMine) return;

        base.Check();
       
    }

    [PunRPC]
    public override void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = PhotonNetwork.Instantiate(mpPath+sale.gun.name, player.transform.position, Quaternion.identity);
        gun.transform.parent = pl.weaponsPrefab.transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.weapons.Count == 1) player.GetComponent<PlayerController>().GunSwap();
        if (pl.gunCapacity == pl.weapons.Count)
        {
            PhotonNetwork.Destroy(pl.weapons[pl.currentGun].gameObject);
            pl.weapons.RemoveAt(pl.currentGun);
        }
        pl.weapons.Add(gun.GetComponent<Gun>());
        pl.currentGun = (pl.currentGun + 1) % pl.gunCapacity;
        gun.GetComponent<PhotonView>().RPC("Sync", RpcTarget.Others, mp.playerIndex);
        weaponUI.ImageUpdate();
        player.GetComponent<PlayerController>().GunSwap();
        player.GetComponent<PlayerController>().GunSwap();

        pl.coins -= sale.cost;
    }

}
