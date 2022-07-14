using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Shooting : MonoBehaviour, IAttack
{
    public GameObject bullet;
    protected Gun gun;
    protected DefaultMeleeWeapon defWeapon;
    public MMFeedbacks shotFeedback;

    void Start()
    {
        gun = GetComponent<Gun>();
        defWeapon = gun.owner.GetComponent<DefaultMeleeWeapon>();
    }

    public virtual void Attack()
    {
        var enemy = Physics2D.OverlapCircle(transform.position + (Vector3)defWeapon.attackRangeCenter, defWeapon.attackRange, gun.owner.GetComponent<PlayerController>().enemyMask);
        if (enemy != null)
        {
            defWeapon.GetComponent<IAttack>().Attack();
            gun.canAttack = false;
        }

        if (!gun.canAttack || gun.isReloading || gun.curAmmo <= 0) return;

        var _bullet = Instantiate(bullet, gameObject.transform.position + gameObject.transform.right / 2, gameObject.transform.rotation);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        gun.canAttack = false;
        gun.curAmmo--;
        shotFeedback.PlayFeedbacks(transform.position);
    }
}
