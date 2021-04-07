using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    void Start()
    {
        var playerPos = GameObject.Find("Player").transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
    void Update()
    {
        //transform.position = new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, -10);
        var playerPos = GameObject.Find("Player").transform.position;
        transform.position = Vector3.Lerp(transform.position, new Vector3(playerPos.x, playerPos.y, transform.position.z), 0.02f);
    }
}
