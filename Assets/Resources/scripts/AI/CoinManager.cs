using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    GameObject player;
    public float moveDistance, speed;
    public int cost;
    AudioSource sound;
    Rigidbody2D rigidbody;
    public GameObject particle;
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
        StartCoroutine(ToTrigger());
    }

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= moveDistance) rigidbody.velocity = (player.transform.position - transform.position).normalized * speed;
        else rigidbody.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            var part = Instantiate(particle, gameObject.transform.position, Quaternion.identity);
            player.GetComponent<Player>().coins += cost;
            //sound.PlayOneShot(sound.clip);
            GameObject.Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            var part = Instantiate(particle, gameObject.transform.position, Quaternion.identity);
            player.GetComponent<Player>().coins += cost;
            //sound.PlayOneShot(sound.clip);
            GameObject.Destroy(gameObject);
        }
    }

    IEnumerator ToTrigger()
    {
        yield return new WaitForSeconds(.8f);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

}
