using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consrtuctions : MonoBehaviour
{
    public GameObject LWall, RWall, UWall, DWall, LUCorner, RUCorner, RDCorner, LDCorner;

    [HideInInspector] public bool lW = false, rW = false, uW = false, dW = false, lUC = false, rUC = false, rDC = false, lDC = false;
}
