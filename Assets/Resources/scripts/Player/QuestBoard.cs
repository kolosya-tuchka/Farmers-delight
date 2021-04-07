using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    Animator animator;
    bool isOpen = false, canOpen = false;
    GameObject panel;
    void Start()
    {
        panel = GameObject.Find("QuestPanel");
        animator = GameObject.Find("QuestWindow").GetComponent<Animator>();
        //animator.gameObject.SetActive(false);
        panel.SetActive(false);
    }

    void Update()
    {
        if (canOpen && !isOpen && Input.GetKeyDown(KeyCode.F))
        {
            Trigger();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canOpen = false;
            if (isOpen)
            {
                Trigger();
            }
        }
    }

    public void Trigger()
    {
        //animator.gameObject.SetActive(true);
        animator.SetTrigger("Triggered");
        isOpen = !isOpen;
        panel.SetActive(isOpen);
    }    
}
