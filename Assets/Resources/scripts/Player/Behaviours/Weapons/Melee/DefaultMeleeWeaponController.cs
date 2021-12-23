using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeWeaponController : WeaponController, IAttack
{
    DefaultMeleeWeapon weapon;
    void Awake()
    {
        OnStart();
        weapon = GetComponent<DefaultMeleeWeapon>();
    }

    void Update()
    {
        FixAnim();
    }

    public void FixAnim()
    {
        Weapon curWeapon = player.weapons[player.currentGun];
        weapon.attackRangeCenter = curWeapon.transform.right * 0.6f;

        weapon.anim.transform.position = weapon.transform.position + curWeapon.transform.right * 0.6f;
        weapon.anim.transform.rotation = curWeapon.transform.rotation;
    }

    public void Attack()
    {
        if (!weapon.canAttack) return;

        var enemy = Physics2D.OverlapCircle(transform.position + (Vector3)weapon.attackRangeCenter, weapon.attackRange, player.GetComponent<PlayerController>().enemyMask);
        if (enemy.GetComponent<Enemy>().state == Enemy.State.dead)
        {
            return;
        }
        enemy.GetComponent<Directioner>().TakeDamage(weapon.damage);
        weapon.anim.SetTrigger("Attack");
        weapon.canAttack = false;
    }

    IEnumerator DelayCheck()
    {
        while (true)
        {
            if (!weapon.canAttack)
            {
                yield return new WaitForSeconds(1f / weapon.attackRate / player.shootingBoost);
                weapon.canAttack = true;
            }
            yield return null;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DelayCheck());
    }
}
