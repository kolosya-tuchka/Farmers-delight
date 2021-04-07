﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerAnimations : Animations
{
    float hp1, hp2;
    public MMFeedbacks damageFeedback;
    void Start()
    {
        hp1 = GetComponent<HP>().healPoints;
        hp2 = hp1;
        //renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        hp1 = GetComponent<HP>().healPoints;
        if (hp1 < hp2)
        {
            damageFeedback.PlayFeedbacks(gameObject.transform.position);
            //StartCoroutine(Hit());
        }
        hp2 = hp1;
    }
}