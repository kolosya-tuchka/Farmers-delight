using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaleSign : MonoBehaviour
{
    [SerializeField] GunSale sale;
    [SerializeField] GameObject player;
    [SerializeField] Text text;
    [SerializeField] SpriteRenderer image;
    enum BuyState { simple, restore };
    BuyState state = BuyState.simple;

    void Start()
    {
        sale = GetComponent<GunSale>();
        image = transform.Find("WeaponImage").GetComponent<SpriteRenderer>();
        image.sprite = sale.gun.GetComponent<Gun>().model.sprite;
        text = transform.Find("Cost").transform.Find("Text").GetComponent<Text>();
        text.text = sale.cost.ToString();
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
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
        }

        if (player != null)
        {
            var pl = player.GetComponent<Player>();
            if (pl.target == gameObject && (Input.GetKeyDown(KeyCode.F) || player.GetComponent<Player>().used))
            {
                if (pl.coins >= sale.cost && state == BuyState.simple) Buy();
                else if (pl.coins >= sale.restoreCost && state == BuyState.restore) Restore();
            }
        }
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

    void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = Instantiate(sale.gun);
        gun.transform.parent = player.transform.Find("Weapons").transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.guns.Count == 1) player.GetComponent<PlayerController>().GunSwap();
        if (pl.gunCapacity == pl.guns.Count)
        {
            GameObject.Destroy(pl.guns[pl.currentGun].gameObject);
            pl.guns.RemoveAt(pl.currentGun);
        }
        pl.guns.Add(gun.GetComponent<Gun>());
        pl.currentGun = (pl.currentGun + 1) % pl.gunCapacity;
        GameObject.Find("Weapon").GetComponent<GunManager>().ImageUpdate();
        player.GetComponent<PlayerController>().GunSwap();

        pl.coins -= sale.cost;
    }

    void Restore()
    {
        var pl = player.GetComponent<Player>();
        var g = pl.guns[pl.currentGun].GetComponent<Gun>();
        g.magazines = g.maxMagazines;

        pl.coins -= sale.restoreCost;
    }
}
