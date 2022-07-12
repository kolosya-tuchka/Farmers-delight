using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSale : WeaponSale
{
    private enum BuyState { simple, restore };
    [SerializeField] private BuyState state = BuyState.simple;

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        Check();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);
            player = collision.gameObject;
            player.GetComponent<Player>().target = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(false);
            player = null;
        }
    }

    protected override void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = Instantiate(sign.item);
        gun.transform.parent = pl.weaponsPrefab.transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.gunCapacity == pl.weapons.Count)
        {
            Destroy(pl.weapons[pl.currentGun].gameObject);
            pl.weapons.RemoveAt(pl.currentGun);
        }
        pl.weapons.Add(gun.GetComponent<Gun>());
        pl.currentGun = pl.weapons.Capacity - 2;
        gun.GetComponent<Gun>().owner = pl;
        GameObject.Find("Weapon").GetComponent<GunManager>().ImageUpdate();

        pl.coins -= sign.cost;
    }

    protected virtual void Restore(Gun gun)
    {
        var pl = player.GetComponent<Player>();
        if (gun.magazines == gun.maxMagazines) return;
        gun.magazines = gun.maxMagazines;

        pl.coins -= sign.restoreCost;
    }

    protected override void Check()
    {
        if (player == null) return;

        var p = player.GetComponent<Player>();
        Gun gun = null;   
        state = BuyState.simple;

        foreach (var g in p.weapons)
        {
            if (g.weaponName == sign.item.GetComponent<Gun>()?.weaponName)
            {
                state = BuyState.restore;
                gun = (Gun)g;
                break;
            }
        }

        text.text = (state == BuyState.simple ? sign.cost : sign.restoreCost).ToString();

        if (p.target == gameObject && (Input.GetKeyDown(KeyCode.F) || player.GetComponent<Player>().used))
        {
            if (p.coins >= sign.cost && state == BuyState.simple) Buy();
            else if (p.coins >= sign.restoreCost && state == BuyState.restore) Restore(gun);
        }
    }

}
