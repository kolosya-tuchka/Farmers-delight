using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkManager : MonoBehaviour
{
    Perk perk;
    public Perks perks;
    bool canBuy
    {
        get
        {
            return perks.player.coins >= perk.cost * perks.costMultiplation;
        }
    }

    void Start()
    {
        perk = GetComponent<Perk>();

        switch (perk.type)
        {
            case Perk.Type.hp: perk.title = "healpoints boost "; break;
            case Perk.Type.movement: perk.title = "movement boost "; break;
            case Perk.Type.reload: perk.title = "reload boost "; break;
            case Perk.Type.tap: perk.title = "shooting boost "; break;
        }

    }

    void Update()
    {
        if (canBuy && perks.player.target == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.F) || perks.player.used)  Buy();
        }
    }

    void Buy()
    {
        GetComponent<SpriteRenderer>().sprite = perks.emptyMug;
        switch (perk.type)
        {
            case Perk.Type.hp: perks.player.health.maxHP *= 2; break;
            case Perk.Type.movement: perks.player.speedBoost *= 1.35f; break;
            case Perk.Type.reload: perks.player.reloadBoost *= 2.5f; break;
            case Perk.Type.tap: perks.player.shootingBoost *= 2; break;
        }
        perks.player.coins -= perk.cost * (int)perks.costMultiplation;
        perks.costMultiplation *= 2;
        perks.player.target = null;
        perks.tips.UpdateTip(null);

        var perkUI = Instantiate(perks.perkUIPrefab, perks.perksUI.transform);
        perkUI.GetComponent<PerkUI>().image.color = perk.perkColor;
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == perks.player.gameObject)
        {
            perks.tips.UpdateTip("Press F to buy a " + perk.title + "for " + (perk.cost * perks.costMultiplation).ToString());
            perks.player.target = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == perks.player.gameObject)
        {
            if (perks.player.target == gameObject)
                perks.tips.UpdateTip(null);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canBuy && perks.player.target == gameObject)
        {
            if (Input.GetKeyDown(KeyCode.F) || perks.player.used) Buy();
        }
    }

}
