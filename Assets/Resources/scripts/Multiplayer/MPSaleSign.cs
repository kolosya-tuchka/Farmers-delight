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
        image = transform.Find("WeaponImage").GetComponent<SpriteRenderer>();
        image.sprite = sale.gun.GetComponent<Gun>().model.sprite;
        text = transform.Find("Cost").transform.Find("Text").GetComponent<Text>();
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

        var p = player.GetComponent<Player>();
        var g = p.guns[p.currentGun];

        if (g.name == sale.gun.GetComponent<Gun>().name)
        {
            state = BuyState.restore;
            text.text = sale.restoreCost.ToString();
        }
        else
        {
            state = BuyState.simple;
            text.text = sale.cost.ToString();
        }
       
        if (p.target == gameObject && (Input.GetKeyDown(KeyCode.F) || p.used))
        {
            if (p.coins >= sale.cost && state == BuyState.simple) Buy();
            else if (p.coins >= sale.restoreCost && state == BuyState.restore) Restore();
        }
       
    }

    [PunRPC]
    public override void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = PhotonNetwork.Instantiate(mpPath+sale.gun.name, player.transform.position, Quaternion.identity);
        gun.transform.parent = pl.weapons.transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.guns.Count == 1) player.GetComponent<PlayerController>().GunSwap();
        if (pl.gunCapacity == pl.guns.Count)
        {
            GameObject.Destroy(pl.guns[pl.currentGun].gameObject);
            pl.guns.RemoveAt(pl.currentGun);
        }
        pl.guns.Add(gun.GetComponent<Gun>());
        pl.currentGun = (pl.currentGun + 1) % pl.gunCapacity;
        gun.GetComponent<PhotonView>().RPC("Sync", RpcTarget.Others, mp.playerIndex);
        GameObject.Find("Weapon").GetComponent<GunManager>().ImageUpdate();
        player.GetComponent<PlayerController>().GunSwap();
        view.RPC("GunSwap", RpcTarget.AllViaServer);

        pl.coins -= sale.cost;
    }

    public override void Restore()
    {
        base.Restore();
    }

}
