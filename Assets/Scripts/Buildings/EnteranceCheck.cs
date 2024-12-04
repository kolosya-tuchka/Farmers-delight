using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteranceCheck : MonoBehaviour
{
    GameObject pub, parent;
    Color enter, exit;
   [HideInInspector] public bool canEnter;
    void Start()
    {
        pub = GameObject.Find("Pub");
        parent = transform.parent.gameObject;
        var sprite = parent.GetComponent<SpriteRenderer>();
        enter = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.1f);
        exit = sprite.color;
    }

    void Update()
    {
        var sprite = parent.GetComponent<SpriteRenderer>();
        var coll = parent.GetComponent<BoxCollider2D>();
        var manager = GameObject.Find("Game Manager").GetComponent<RoundManager>();
        var tr = coll.gameObject.transform.Find("Trigger").GetComponent<TrAlpha>();
        if (manager.isBreak && canEnter)
        {
            sprite.color = Color.Lerp(sprite.color, enter, 1 * Time.deltaTime);
            coll.enabled = false;
            //pub.SetActive(true);
        }
        else
        {
            coll.enabled = true;
            if (tr.contacts == 0) sprite.color = Color.Lerp(sprite.color, exit, 1 * Time.deltaTime);
            //pub.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) canEnter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) canEnter = false;
    }
}
