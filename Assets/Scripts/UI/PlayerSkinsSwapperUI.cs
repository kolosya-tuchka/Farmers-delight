using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinsSwapperUI : MonoBehaviour
{
    public GameObject[] skins;
    private int skin;
    void Start()
    {
        skin = PlayerPrefs.GetInt("PlayerSkin");
        skins[skin].SetActive(true);
    }

    public void SkinSawp()
    {
        skins[skin].SetActive(false);
        skin = (skin + 1) % skins.Length;
        skins[skin].SetActive(true);
        PlayerPrefs.SetInt("PlayerSkin", skin);
    }
}
