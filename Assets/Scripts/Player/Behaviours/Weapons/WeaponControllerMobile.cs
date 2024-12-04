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

        switch (weapon.moveMode)
        {
            case Weapon.MoveMode.rotate:
                {
                    Vector2 target = pc.targetEnemy == null ? rb.velocity : (Vector2)pc.targetEnemy.transform.position - (Vector2)transform.position;
                    float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                    weapon.transform.eulerAngles = new Vector3(0f, 0f, angle);
                    weapon.model.flipY = weapon.transform.right.x < 0;
                    break;
                }
            case Weapon.MoveMode.flip:
                {

                    break;
                }
        }
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
