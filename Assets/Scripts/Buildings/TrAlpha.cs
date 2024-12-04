using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrAlpha : MonoBehaviour
{
    SpriteRenderer color;
    Color enterCl, exitCl;
    [HideInInspector] public int contacts = 0;
    void Start()
    {
        color = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        enterCl = new Color(color.color.r, color.color.g, color.color.b, 0.75f);
        exitCl = new Color(color.color.r, color.color.g, color.color.b, color.color.a);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            contacts--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            contacts++;
        }
    }

    void FixedUpdate()
    {
        var check = color.gameObject.transform.Find("Enterance").GetComponent<EnteranceCheck>();
        if (contacts == 0 && !check.canEnter) color.color = Color.Lerp(color.color, exitCl, 1 * Time.deltaTime);
        else if (contacts != 0) color.color = Color.Lerp(color.color, enterCl, 1 * Time.deltaTime);
    }

}
