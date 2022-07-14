using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MPGunSale : GunSale
{
    PhotonView view;
    MPManager mp;
    [SerializeField] string mpPath = "Multiplayer/Guns/";
    bool canBuy;
    void Start()
    {
        canBuy = false;
        mp = FindObjectOfType<MPManager>();
        player = mp.player;
        view = player.GetComponent<PhotonView>();

        sign = GetComponent<SaleSign>();
        image.sprite = sign.item.GetComponent<Weapon>().model.sprite;
        text.text = sign.cost.ToString();
        text.gameObject.SetActive(false);
        if (weaponUI == null)
        {
            weaponUI = FindObjectOfType<GunManager>();
        }
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
            player.GetComponent<Player>().target = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            canBuy = false;
            text.gameObject.SetActive(false);
            var p = player.GetComponent<Player>();
            if (p.target == gameObject)
            {
                p.target = null;
            }
        }
    }

    protected override void Check()
    {
        if (!view.IsMine) return;

        base.Check();
       
    }
    
    protected override void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = PhotonNetwork.Instantiate(mpPath + sign.item.name, player.transform.position, Quaternion.identity);
        int curGun = pl.currentGun;
        gun.transform.parent = pl.weaponsPrefab.transform;
        gun.transform.localPosition = gun.GetComponent<Weapon>().localPos;
        if (pl.gunCapacity == pl.weapons.Count)
        {
            if (curGun != 0) PhotonNetwork.Destroy(pl.weapons[curGun].gameObject);
            else pl.weapons[curGun].gameObject.SetActive(false);
            pl.weapons[curGun] = gun.GetComponent<Weapon>();
            pl.currentGun = (pl.gunCapacity + curGun - 1) % pl.gunCapacity;
        }
        else
        {
            pl.weapons.Add(gun.GetComponent<Weapon>());
            pl.currentGun = pl.weapons.Count - 2;
        }
        gun.GetComponent<Weapon>().owner = pl;
        gun.GetComponent<PhotonView>().RPC("Sync", RpcTarget.Others, MPManager.playerIndex, curGun);
        weaponUI.ImageUpdate();

        pl.coins -= sign.cost;
    }

}
