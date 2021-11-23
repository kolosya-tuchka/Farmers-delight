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
        var mp = FindObjectOfType<MPManager>();
        if (mp != null) player = mp.player;
        else player = FindObjectOfType<Player>().gameObject;

        rigidbody = GetComponent<Rigidbody2D>();
        sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= moveDistance) rigidbody.velocity = (player.transform.position - transform.position).normalized * speed;
        else rigidbody.velocity = Vector2.zero;
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

}
