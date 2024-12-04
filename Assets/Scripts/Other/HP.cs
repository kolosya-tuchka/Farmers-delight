using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HP : MonoBehaviour
{
    public float healPoints, maxHP, regenerationSpeed, delayOfRegeneration, delayTimeLeft;
    public bool canRegenerate;
    public Image bar;
    protected virtual void Start()
    {
        healPoints = maxHP;
    }

    
    protected virtual void Update()
    {
        RenderHP();

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

    protected virtual void RenderHP()
    {
        if (bar != null)
        {
            bar.fillAmount = healPoints / maxHP;
        }
    }

    protected void Regeneration()
    {
        if (delayTimeLeft == 0)
        {
            if (healPoints < maxHP)
            {
                healPoints += regenerationSpeed * Time.deltaTime;
            }

            healPoints = Mathf.Clamp(healPoints, 0, maxHP);
        }
    }
}
