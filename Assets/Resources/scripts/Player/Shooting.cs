using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public Gun gun;
    public float shootRate;
    public float sR = 0, shootRate1;
    public MMFeedbacks shotFeedback;
    ShootButton btn;

    void Start()
    {
       // btn = GameObject.Find("ShootButton").GetComponent<ShootButton>();
        gun = GetComponent<Gun>();
    }

    void Update()
    {
        ShotCheck();
    }

    public virtual void Shot()
    {
        var _bullet = Instantiate(bullet, gameObject.transform.position + gameObject.transform.right / 2, gameObject.transform.rotation);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        sR += shootRate1;
        gun.curAmmo--;
        shotFeedback.PlayFeedbacks(transform.position);
    }

    public virtual void ShotCheck()
    {
        shootRate1 = shootRate / GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().shootingBoost;
        if (sR <= 0 && gun.curAmmo > 0 && !gun.isReloading)
        {
            switch (SystemInfo.deviceType)
            {
                case (DeviceType.Desktop):
                    {
                        if (Input.GetMouseButton(0))
                        {
                            Shot();
                        }
                        break;
                    }
                case (DeviceType.Handheld):
                    {
                        var controller = GetComponent<WeaponController>();
                        if (controller.joystick.Direction.magnitude > 0.6f)
                        {
                            Shot();
                        }

                        //if (btn.canShoot)
                        //    Shot();

                        break;
                    }
            }
        }
        else if (sR > 0) sR -= Time.deltaTime;
    }
}
