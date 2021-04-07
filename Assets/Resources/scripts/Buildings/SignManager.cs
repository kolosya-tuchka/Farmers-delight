using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignManager : MonoBehaviour
{
    GunSale sale;
    GameObject player;
    public Text text;
    void Start()
    {
        sale = GetComponent<GunSale>();
        text.text = sale.cost.ToString();
    }

    void Update()
    {
        if (player != null)
        {
            var pl = player.GetComponent<Player>();
            if (Input.GetKeyDown(KeyCode.F) && pl.coins >= sale.cost) Buy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);
            player = collision.gameObject;
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

    void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = Instantiate(sale.gun);
        gun.transform.parent = player.transform.Find("Weapons").transform;
        gun.transform.localPosition = gun.GetComponent<Gun>().localPos;
        if (pl.gunCapacity == pl.guns.Count)
        {
            GameObject.Destroy(pl.guns[pl.currentGun].gameObject);
            pl.guns.RemoveAt(pl.currentGun);
        }
        pl.guns.Add(gun.GetComponent<Gun>());
        pl.currentGun = (pl.currentGun + 1) % pl.gunCapacity;
        GameObject.Find("Weapon").GetComponent<GunManager>().ImageUpdate();
        pl.coins -= sale.cost;
    }
}
