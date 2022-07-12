using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MeleeAttack : MonoBehaviour, IAttack
{
    protected MeleeWeapon weapon;
    [SerializeField] protected MMFeedbacks attackFeedback;
    protected Player player;

    void Start()
    {
        weapon = GetComponent<MeleeWeapon>();
        player = weapon.owner;
    }

    public virtual void Attack()
    {
        if (!weapon.canAttack) return;

        var enemies = Physics2D.OverlapCircleAll(transform.position + weapon.offset, weapon.attackRange, player.GetComponent<PlayerController>().enemyMask);
        
        foreach(var e in enemies)
        {
            if (e.GetComponent<Enemy>().state == Enemy.State.dead)
            {
                continue;
            }
            e.GetComponent<IDamage>().TakeDamage(weapon.damage);
        }
        weapon.weaponAnimations.StartCoroutine(weapon.weaponAnimations.RotateWeapon(45, 1 / weapon.attackRate));
        weapon.anim.transform.position = transform.position + weapon.offset;
        weapon.anim.transform.rotation = transform.rotation;
        weapon.anim.SetTrigger("Attack");
        weapon.isFocused = true;
        weapon.canAttack = false;
        attackFeedback.PlayFeedbacks(transform.position);
    }
}
