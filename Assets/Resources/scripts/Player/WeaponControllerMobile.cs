using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponControllerMobile : WeaponController
{
    void Start()
    {
        OnStart();
    }

    void Update()
    {
        Controll();
    }

    public override void Controll()
    {
        var weapon = player.weapons[player.currentGun];
        var pc = player.GetComponent<PlayerController>();
        var rb = player.GetComponent<Rigidbody2D>();

        Vector2 target = pc.targetEnemy == null ? rb.velocity : (Vector2)pc.targetEnemy.transform.position;
        float angle = Vector2.Angle(Vector2.right, target - (Vector2)transform.position);
        weapon.transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < target.y ? angle : -angle);
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            var weapon = player.weapons[player.currentGun];
            weapon.GetComponent<IAttack>().Attack();
            yield return null;
        }
    }
}
