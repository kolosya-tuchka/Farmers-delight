using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBar : MonoBehaviour
{
    Image color;
    Color enterCl, exitCl;
    int contacts = 0;
    void Start()
    {
        color = transform.parent.gameObject.transform.Find("HP").gameObject.transform.Find("HPBar").gameObject.transform.Find("Bar").gameObject.GetComponent<Image>();
        enterCl = new Color(color.color.r, color.color.g, color.color.b, 0.9f);
        exitCl = new Color(color.color.r, color.color.g, color.color.b, 0);
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

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        var bar = color.gameObject.transform.parent;
    //        bar.transform.position = new Vector3(bar.transform.position.x, collision.gameObject.transform.position.y + 2.5f, bar.transform.position.z);
    //    }
    //}

    void FixedUpdate()
    {
        if (contacts == 0) color.color = Color.Lerp(color.color, exitCl, 1 * Time.deltaTime);
        else color.color = Color.Lerp(color.color, enterCl, 1 * Time.deltaTime);
        var cl = color.gameObject.transform.parent.gameObject.transform.Find("Background").GetComponent<Image>().color;
        color.gameObject.transform.parent.gameObject.transform.Find("Background").GetComponent<Image>().color = new Color(cl.r, cl.g, cl.b, color.color.a);
    }
}
