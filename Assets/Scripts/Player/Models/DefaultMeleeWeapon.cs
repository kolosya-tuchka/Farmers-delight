using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultMeleeWeapon : Weapon
{
    public float attackRange;
    public float damage;
    public Animator anim;
    public Vector2 attackRangeCenter;

    void Awake()
    {
        localPos = Vector3.zero;
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        canAttack = true;
    } 

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)attackRangeCenter, attackRange);
    }

}
