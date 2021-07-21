using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MPShooting : Shooting
{
    string mpPath = "Multiplayer/Bullets/";
    void Update()
    {
        ShotCheck();
    }

    public override void Shot()
    {
        var _bullet = PhotonNetwork.Instantiate(mpPath+bullet.name, gameObject.transform.position + gameObject.transform.right / 2, gameObject.transform.rotation);
        _bullet.transform.SetParent(GameObject.Find("Bullets").transform);
        sR += shootRate1;
        gun.curAmmo--;
        shotFeedback.PlayFeedbacks(transform.position);
    }

    public override void ShotCheck()
    {
        if (!GetComponent<PhotonView>().IsMine) return;

        shootRate1 = shootRate / FindObjectOfType<MPManager>().player.GetComponent<Player>().shootingBoost;
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
