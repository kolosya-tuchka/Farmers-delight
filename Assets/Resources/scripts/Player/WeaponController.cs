using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public Joystick joystick;
    SpriteRenderer renderer;
    public Button shootBtn;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
        joystick = GameObject.Find("Gun Joystick").GetComponent<Joystick>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        switch (SystemInfo.deviceType)
        {
            case (DeviceType.Desktop):
                {
                    var mousePosition = Input.mousePosition;
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    var angle = Vector2.Angle(Vector2.right, mousePosition - transform.position);
                    transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < mousePosition.y ? angle : -angle);
                    break;
                }
            case (DeviceType.Handheld):
                {
                    //GameObject obj = player.GetComponent<PlayerController>().SearchClosestEnemy();
                    //Vector3 pos = obj.transform.position;
                    //var angle = Vector2.Angle(Vector2.right, pos - transform.position);
                    //transform.eulerAngles = new Vector3(0f, 0f, transform.position.y < pos.y ? angle : -angle);
                    transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg);
                    break;
                }
        }


    }
}
