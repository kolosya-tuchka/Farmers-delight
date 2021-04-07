using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkManager : MonoBehaviour
{
    bool canBuy;
    Perk perk;
    Player player;
    GameObject tips;
    void Start()
    {
        perk = GetComponent<Perk>();
        player = GameObject.Find("Player").GetComponent<Player>();
        tips = GameObject.Find("Tips");
    }

    void Update()
    {
        if (canBuy && player.coins >= perk.cost && player.target == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.F) || player.used)  Buy();
        }
    }

    void Buy()
    {
        GetComponent<SpriteRenderer>().sprite = perk.emptyMug;
        switch (perk.title)
        {
            case Perk.Title.hp: player.health.maxHP *= 2; break;
            case Perk.Title.movement: player.speed *= 1.5f; break;
            case Perk.Title.reload: player.reloadBoost *= 2; break;
            case Perk.Title.tap: player.shootingBoost *= 2; break;
        }
        player.coins -= perk.cost;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canBuy = true;
            string text = null;
            switch (perk.title)
            {
                case Perk.Title.hp: text = "healpoints boost "; break;
                case Perk.Title.movement: text = "movement boost "; break;
                case Perk.Title.reload: text = "reload boost "; break;
                case Perk.Title.tap: text = "shooting boost "; break;
            }
            tips.GetComponent<Tips>().UpdateTip("Press F to buy a " + text + "for " + (perk.cost).ToString());
            player.GetComponent<Player>().target = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canBuy = false;
            string text = null;
            switch (perk.title)
            {
                case Perk.Title.hp: text = "healpoints boost "; break;
                case Perk.Title.movement: text = "movement boost "; break;
                case Perk.Title.reload: text = "reload boost "; break;
                case Perk.Title.tap: text = "shooting boost "; break;
            }
            if (tips.GetComponent<Text>().text == "Press F to buy a " + text + "for " + (perk.cost).ToString()) GameObject.Find("Tips").GetComponent<Tips>().UpdateTip(null);
        }
    }

}
