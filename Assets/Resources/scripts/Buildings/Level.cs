using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int roomSpaceW = 20, roomSpaceH = 20;
}
public class Room : MonoBehaviour
{
    public int PosX, PosY;
    public Consrtuctions[,] room;
}
