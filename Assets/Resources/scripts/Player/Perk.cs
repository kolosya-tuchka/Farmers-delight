using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    public int cost;
    public Title title;
    public Sprite emptyMug;
    public enum Title
    {
        movement, hp, reload, tap
    }
}
