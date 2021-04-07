using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HP : MonoBehaviour
{
    public float healPoints, maxHP, regenerationSpeed, delayOfRegeneration, delayTimeLeft;
    public bool canRegenerate;
    public Image bar;
    void Start()
    {
        healPoints = maxHP;
    }

    
    void Update()
    {
        if (bar != null)
        {
            bar.fillAmount = healPoints / maxHP;
        }

        if (delayTimeLeft != 0)
        {
            delayTimeLeft -= Time.deltaTime;
        }

        if (delayTimeLeft <= 0)
        {
            delayTimeLeft = 0;
        }

        if (canRegenerate)
        {
            Regeneration();
        }
    }

    void Regeneration()
    {
        if (delayTimeLeft == 0)
        {
            if (healPoints < maxHP)
            {
                healPoints += regenerationSpeed * Time.deltaTime;
            }
        }
    }
}
