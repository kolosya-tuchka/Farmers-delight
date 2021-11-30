using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Shooting : MonoBehaviour, IAttack
{
    public GameObject bullet;
    protected Gun gun;
    public MMFeedbacks shotFeedback;

    void Start()
    {
        gun = GetComponent<Gun>();
    }

    public virtual void Attack()
    {
        if (!gun.canAttack || gun.isReloading) return;

        var _bullet = Instantiate(bullet, gameObject.transform.position + gameObject.transform.right / 2, gameObject.transform.rotation);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        gun.canAttack = false;
        gun.curAmmo--;
        shotFeedback.PlayFeedbacks(transform.position);
    }
}
