using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    void Attack();
}

public class WeaponController : MonoBehaviour
{
    protected Player player;

    void Start()
    {
        OnStart();
    }

    private void Update()
    {
        Controll();
    }

    public virtual void Controll()
    {
        var weapon = player.weapons[player.currentGun];

        var mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);
        weapon.transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? angle : -angle);

        if (Input.GetMouseButton(0))
            weapon.GetComponent<IAttack>().Attack();
    }

    public virtual void OnStart()
    {
        player = GetComponent<Player>();
    }

}
