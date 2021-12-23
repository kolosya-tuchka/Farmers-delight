using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject lobby, start;

    void Start()
    {
        lobby.SetActive(false);
        start.SetActive(true);
    }

    public void Swap()
    {
        lobby.SetActive(!lobby.activeInHierarchy);
        start.SetActive(!start.activeInHierarchy);
    }
}
