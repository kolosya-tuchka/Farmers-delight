using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    public int cost;
    public Type type;
    public Color perkColor;
    [HideInInspector] public string title;
    public enum Type
    {
        movement, hp, reload, tap
    }
}
