using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    Vector2 Mvm;
    Rigidbody2D rigidbody;
    public SpriteRenderer renderer;
    public Animator animator;
    Player player;
    int x, y;
    float time = 1;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        player.currentGun = 0;
        var gunz = GameObject.FindGameObjectsWithTag("Gun");
        foreach (var gun in gunz)
        {
            if (gun.transform.parent.gameObject.transform.parent.gameObject == gameObject) player.guns.Add(gun.GetComponent<Gun>());
        }
        GunSwap();
    }

    void FixedUpdate()
    {
        CheckMove();

        if (player.health.healPoints < 0) GameOver();
        //if (time > 0) time -= Time.deltaTime;
        //else if (player.healPoints < player.maxhp)
        //{
        //    player.healPoints += player.healSpeed;
        //    time = 1;
        //}

        if (Input.GetKeyDown(KeyCode.T)) GunSwap();
    }

    void CheckMove()
    {
        Mvm = new Vector2(0, 0);

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                x = 1;
                renderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                x = -1;
                renderer.flipX = true;
            }
            else x = 0;


            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                y = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                y = -1;
            }
            else y = 0;

            Mvm = new Vector2(x, y).normalized;
        }

        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Mvm = joystick.Direction.normalized;
            renderer.flipX = joystick.Horizontal < 0;
        }
        if (Mvm.x != 0 || Mvm.y != 0) animator.SetBool("isMoving", true);
        else animator.SetBool("isMoving", false);
        rigidbody.velocity = Mvm * player.speed;
    }

    public void GunSwap()
    {
        if (player.guns.Count > 1)
        {
            player.currentGun = (player.currentGun + 1) % player.gunCapacity;
            foreach (var gun in player.guns)
            {
                gun.gameObject.SetActive(false);
            }
            player.guns[player.currentGun].gameObject.SetActive(true);

            StartCoroutine(GameObject.Find("Weapon").GetComponent<GunManager>().ImageUpdate());
        }
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

    void GameOver()
    {
        renderer.gameObject.transform.parent = transform.parent;
        GameObject.Destroy(gameObject);
    }

    public GameObject SearchClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = enemies[0];
        float dist = (transform.position - closest.transform.position).magnitude;
        foreach (var en in enemies)
        {
            if ((transform.position - en.transform.position).magnitude < dist)
            {
                closest = en;
                dist = (transform.position - en.transform.position).magnitude;
            }
        }

        return closest;
    }
}
