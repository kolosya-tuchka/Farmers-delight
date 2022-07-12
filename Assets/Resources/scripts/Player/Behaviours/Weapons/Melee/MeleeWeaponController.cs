using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    protected MeleeWeapon weapon;

    protected float curSpeed;

    private void Awake()
    {
        weapon = GetComponent<MeleeWeapon>();
    }

    private void Start()
    {
        curSpeed = weapon.owner.speed;
        weapon.owner.GetComponent<PlayerController>().GunSwap();
    }
    
    protected IEnumerator DelayCheck()
    {
        while (true)
        {
            if (!weapon.canAttack)
            {
                curSpeed = weapon.owner.speed;
                weapon.owner.speed /= 2;
                yield return new WaitForSeconds(1f / weapon.attackRate / weapon.owner.shootingBoost);
                weapon.canAttack = true;
                weapon.isFocused = false;
                weapon.owner.speed = curSpeed;
            }
            yield return null;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DelayCheck());
    }

    private void OnDisable()
    {
        weapon.owner.speed = curSpeed;
    }
}
