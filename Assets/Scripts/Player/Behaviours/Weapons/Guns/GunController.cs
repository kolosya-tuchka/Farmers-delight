using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour, IReloadable
{
    protected Gun gun;
    public float speedMultiply;
    void Awake()
    {
        gun = GetComponent<Gun>();
    }

    void Start()
    {
        gun.owner.GetComponent<PlayerController>().GunSwap();
    }

    public IEnumerator ReloadCheck()
    {
        while (true)
        {
            yield return null;
            if (gun.canReload && gun.magazines > 0)
            {
                gun.isReloading = true;
                gun.reloadProgress = 0;

                while (gun.reloadProgress < gun.reloadTime / gun.owner.reloadBoost)
                {
                    gun.reloadProgress += Time.deltaTime;
                    yield return null;
                }

                gun.isReloading = false;
                gun.magazines--;
                gun.curAmmo = gun.maxAmmo;
            }
        }
    }

    protected IEnumerator DelayCheck()
    {
        while (true)
        {
            yield return null;
            if (!gun.canAttack)
            { 
                gun.owner.curSpeed *= speedMultiply;
                yield return new WaitForSeconds(1f / gun.attackRate / gun.owner.shootingBoost);
                gun.canAttack = true;
                gun.owner.ResetSpeed();
            }
        }
    }

    private void OnEnable()
    {
        gun.owner.ResetSpeed();
        StartCoroutine(ReloadCheck());
        StartCoroutine(DelayCheck());
    }

    private void OnDisable()
    {
        gun.owner.ResetSpeed();
        gun.isReloading = false;
    }

}

public interface IReloadable
{
    IEnumerator ReloadCheck();
}
