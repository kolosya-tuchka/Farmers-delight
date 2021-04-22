using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefObj : MonoBehaviour
{
    public HP hp;
    void Update()
    {
        if (hp.healPoints <= 0)
        {
            FindObjectOfType<InterfaceManager>().GetComponent<InterfaceManager>().GameOver();
            FindObjectOfType<PlayerController>().GameOver();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") collision.gameObject.GetComponent<Enemy>().canDestroy = true;
    }

    ////private void OnCollisionExit2D(Collision2D collision)
    ////{
    ////    if (collision.gameObject.tag == "Enemy") collision.gameObject.GetComponent<Enemy>().canDestroy = false;
    ////}
}
