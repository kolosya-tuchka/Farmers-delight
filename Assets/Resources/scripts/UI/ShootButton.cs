using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public bool canShoot = false;
    private void OnMouseDown()
    {
        canShoot = true;
    }

    private void OnMouseUp()
    {
        canShoot = false;
    }
}
