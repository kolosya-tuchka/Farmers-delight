using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    MPManager mp;

    public Image progress, image;
    public Text magazineText;
    Weapon curWeapon;
    Player player;
    void Start()
    {
        mp = FindObjectOfType<MPManager>();

        if (mp != null) player = mp.player.GetComponent<Player>();
        else player = FindObjectOfType<Player>();
        ImageUpdate();
    }

    void Update()
    {
        if (curWeapon.GetComponent<IReloadable>() != null)
        {
            Gun g = curWeapon.GetComponent<Gun>();
            if (g.isReloading) progress.fillAmount = g.reloadProgress / g.reloadTime;
            else progress.fillAmount = g.curAmmo / g.maxAmmo;
            magazineText.text = g.magazines.ToString();
        }
        else
        {
            progress.fillAmount = 1;
            magazineText.text = null;
        }

    }

    public void ImageUpdate()
    {
        curWeapon = player.weapons[player.currentGun];
        image.sprite = curWeapon.model.sprite;
    }
}
