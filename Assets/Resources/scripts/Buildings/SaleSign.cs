using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleSign : MonoBehaviour
{
    public GunManager weaponUI;
    public GunSale sale;
    public GameObject player;
    public Text text;
    public SpriteRenderer image;
    public enum BuyState { simple, restore };
    public BuyState state = BuyState.simple;

    void Start()
    {
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
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);
            player = collision.gameObject;
        }
        player.GetComponent<Player>().target = gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(false);
            player = null;
        }
    }

    public virtual void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = Instantiate(sale.gun);
        gun.transform.parent = pl.weaponsPrefab.transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.weapons.Count == 1) player.GetComponent<PlayerController>().GunSwap();
        if (pl.gunCapacity == pl.weapons.Count)
        {
            Destroy(pl.weapons[pl.currentGun].gameObject);
            pl.weapons.RemoveAt(pl.currentGun);
        }
        pl.weapons.Add(gun.GetComponent<Gun>());
        pl.currentGun = (pl.currentGun + 1) % pl.gunCapacity;
        gun.GetComponent<Gun>().owner = pl;
        GameObject.Find("Weapon").GetComponent<GunManager>().ImageUpdate();
        player.GetComponent<PlayerController>().GunSwap();
        player.GetComponent<PlayerController>().GunSwap();

        pl.coins -= sale.cost;
    }

    public virtual void Restore(Gun gun)
    {
        var pl = player.GetComponent<Player>();
        if (gun.magazines == gun.maxMagazines) return;
        gun.magazines = gun.maxMagazines;

        pl.coins -= sale.restoreCost;
    }

    public virtual void Check()
    {
        if (player == null) return;

        var p = player.GetComponent<Player>();
        Gun gun = null;   
        state = BuyState.simple;

        foreach (var g in p.weapons)
        {
            if (g.weaponName == sale.gun.GetComponent<Gun>().weaponName)
            {
                state = BuyState.restore;
                gun = (Gun)g;
                break;
            }
        }

        text.text = (state == BuyState.simple ? sale.cost : sale.restoreCost).ToString();

        if (p.target == gameObject && (Input.GetKeyDown(KeyCode.F) || player.GetComponent<Player>().used))
        {
            if (p.coins >= sale.cost && state == BuyState.simple) Buy();
            else if (p.coins >= sale.restoreCost && state == BuyState.restore) Restore(gun);
        }
    }

}
