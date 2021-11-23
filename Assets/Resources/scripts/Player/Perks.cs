using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perks : MonoBehaviour
{
    public Tips tips;
    public Player player;
    public GameObject perksUI;
    public GameObject perkUIPrefab;
    public Sprite emptyMug;
    public float costMultiplation;

    void Start()
    {
        costMultiplation = 1;
        var mp = FindObjectOfType<MPManager>();
        if (mp != null) mp.player.GetComponent<Player>();
    }
}
