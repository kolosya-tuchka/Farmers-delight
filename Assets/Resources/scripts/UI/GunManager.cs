using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    public Player player;
    GameObject gun;
    public Image progress, image;
    public Text magazineText;
    Gun gu;
    void Start()
    {
        StartCoroutine(ImageUpdate());
    }

    void Update()
    {
        if (gu.isReloading) progress.fillAmount = gu.reloadProgress / gu.rt;
        else progress.fillAmount = gu.curAmmo / gu.maxAmmo;

        magazineText.text = gu.magazines.ToString();
    }

    public IEnumerator ImageUpdate()
    {
        yield return null;
        gun = player.guns[player.currentGun].gameObject;
        gu = gun.GetComponent<Gun>();
        image.sprite = gu.model.sprite;
    }
}
