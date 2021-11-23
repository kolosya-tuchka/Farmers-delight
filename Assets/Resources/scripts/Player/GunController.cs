using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Gun gun;
    void Awake()
    {
        gun = GetComponent<Gun>();
    }

    IEnumerator ReloadCheck()
    {
        while (true)
        {
            if (gun.canReload)
            {
                gun.isReloading = true;
                gun.magazines--;
                gun.reloadProgress = 0;

                while (gun.reloadProgress < gun.reloadTime / gun.owner.reloadBoost)
                {
                    gun.reloadProgress += Time.deltaTime;
                    yield return null;
                }

                gun.isReloading = false;
                gun.curAmmo = gun.maxAmmo;
            }
            yield return null;
        }
    }

    IEnumerator DelayCheck()
    {
        while (true)
        {
            if (!gun.canAttack)
            {
                yield return new WaitForSeconds(1f / gun.attackRate / gun.owner.shootingBoost);
                gun.canAttack = true;
            }
            yield return null;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ReloadCheck());
        StartCoroutine(DelayCheck());
    }

    private void OnDisable()
    {
        gun.isReloading = false;
    }
}
