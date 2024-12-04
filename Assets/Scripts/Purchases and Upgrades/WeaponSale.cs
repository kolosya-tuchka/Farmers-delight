using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSale : MonoBehaviour
{
    [SerializeField] protected GunManager weaponUI;
    [SerializeField] protected SaleSign sign;
    [SerializeField] protected Text text;
    [SerializeField] protected SpriteRenderer image;
    protected GameObject player;

    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
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

    protected virtual void Buy()
    {
        var pl = player.GetComponent<Player>();
        var gun = Instantiate(sign.item);
        gun.transform.parent = pl.weaponsPrefab.transform;
        gun.transform.localPosition = gun.GetComponent<Weapon>().localPos;
        if (pl.gunCapacity == pl.weapons.Count)
        {
            Destroy(pl.weapons[pl.currentGun].gameObject);
            pl.weapons.RemoveAt(pl.currentGun);
        }
        pl.weapons.Add(gun.GetComponent<Weapon>());
        pl.currentGun = pl.weapons.Capacity - 2;
        gun.GetComponent<Weapon>().owner = pl;
        weaponUI.ImageUpdate();

        pl.coins -= sign.cost;
    }

    protected virtual void Check()
    {
        if (player == null) return;

        var p = player.GetComponent<Player>();
        text.text = sign.cost.ToString();

        if (p.target == gameObject && (Input.GetKeyDown(KeyCode.F) || player.GetComponent<Player>().used))
        {
            if (p.coins >= sign.cost) Buy();
        }
    }
}
