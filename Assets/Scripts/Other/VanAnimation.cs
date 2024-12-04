using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanAnimation : MonoBehaviour
{
    bool isBreak;
    RoundManager manager;
    Animator animator;
    void Start()
    {
        manager = GameObject.Find("Game Manager").GetComponent<RoundManager>();
        isBreak = manager.isBreak;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isBreak != manager.isBreak)
        {
            animator.SetTrigger("Action");
            isBreak = manager.isBreak;
        }
    }
}
