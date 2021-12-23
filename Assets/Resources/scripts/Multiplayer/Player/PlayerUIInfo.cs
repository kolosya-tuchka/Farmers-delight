using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerUIInfo : MonoBehaviour
{
    public Text nickname;
    public Image hpBar;
    private void Start()
    {
        var view = GetComponent<PhotonView>();
        hpBar.transform.parent.gameObject.SetActive(!view.IsMine);
        if (view.IsMine)
        {
            nickname.color = Color.green;
        }
        else
        {
            nickname.color = Color.blue;
            GetComponent<HP>().bar = hpBar;
        }
    }

    public void Update()
    {
        nickname.text = gameObject.name;
    }
}
