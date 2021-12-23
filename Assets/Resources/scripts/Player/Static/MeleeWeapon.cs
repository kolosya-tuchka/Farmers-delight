using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeleeWeapon : DefaultMeleeWeapon
{
    [HideInInspector] public Vector3 offset { get; protected set; }
    [HideInInspector] public WeaponAnimations weaponAnimations { get; protected set; }

    void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        anim = owner.GetComponent<DefaultMeleeWeapon>().anim;
    }

    private void Update()
    {
        FixOffset();
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        weaponAnimations = GetComponent<WeaponAnimations>();
        FixOffset();
    }

    protected void FixOffset()
    {
        switch (moveMode)
        {
            case MoveMode.rotate:
                {
                    offset = transform.right * attackRangeCenter.x + transform.up * attackRangeCenter.y;
                    break;
                }
            case MoveMode.flip:
                {
                    offset = (Vector3)attackRangeCenter * (model.flipX ? 1 : -1);
                    break;
                }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(offset * (model.flipX ? 1 : -1) + transform.position, attackRange);
    }
}
