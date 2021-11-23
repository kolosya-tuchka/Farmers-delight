using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    MPManager mp;

    GameObject gun;
    public Image progress, image;
    public Text magazineText;
    Gun gu;
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
        if (gu.isReloading) progress.fillAmount = gu.reloadProgress / gu.reloadTime;
        else progress.fillAmount = gu.curAmmo / gu.maxAmmo;

        magazineText.text = gu.magazines.ToString();
    }

    public void ImageUpdate()
    {
        gun = player.weapons[player.currentGun].gameObject;
        gu = gun.GetComponent<Gun>();
        image.sprite = gu.model.sprite;
    }
}
