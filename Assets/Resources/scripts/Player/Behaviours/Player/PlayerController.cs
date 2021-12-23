using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    protected Player player;
    protected Rigidbody2D rb;

    public LayerMask enemyMask;
    public Enemy targetEnemy = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        player.currentGun = 0;
        GunSwap();

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            gameObject.AddComponent<PlayerMoves>();
            gameObject.AddComponent<WeaponController>();
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            var mobile = FindObjectOfType<MobileUI>();
            var moves = gameObject.AddComponent<PlayerMovesMobile>();
            var weaponControls = gameObject.AddComponent<WeaponControllerMobile>();

            moves.joystick = mobile.joystick.GetComponent<Joystick>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) GunSwap();
    }

    public virtual void GunSwap()
    {
        if (!player.weapons[player.currentGun].canAttack)
        {
            return;
        }

        if (player.weapons.Count > 1)
        {
            player.currentGun = (player.currentGun + 1) % player.gunCapacity;
            foreach (var gun in player.weapons)
            {
                gun.gameObject.SetActive(false);
            }
            player.weapons[player.currentGun].gameObject.SetActive(true);

            FindObjectOfType<GunManager>().ImageUpdate();
        }
    }

    public virtual Enemy GetEnemyNearby(float radius)
    {
        if (radius <= 0) return null;

        var enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
        foreach (var e in enemies)
        {
            if (e.GetComponent<Enemy>().state == Enemy.State.alive)
            {
                return e.GetComponent<Enemy>();
            }
        }
        return null;
    }

    IEnumerator Use()
    {
        player.used = true;
        yield return new WaitForEndOfFrame();
        player.used = false;
    }

    public void StartUse()
    {
        StartCoroutine(Use());
    }

    public virtual void GameOver()
    {
        //GetComponent<Renderer>().gameObject.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        player.health.healPoints -= damage;
        player.health.delayTimeLeft = player.health.delayOfRegeneration;
        player.anim.HitAhimation();

        if (player.health.healPoints < 0) GameOver();
    }

}
