using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCardUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI nick, kills;
    public Image aliveStateImage;
    public Color otherPlayerNickColor, minePlayerNickColor, aliveColor, deadColor;

    void Update()
    {
        kills.text = player.kills.ToString();
        aliveStateImage.color = player.isAlive ? aliveColor : deadColor;
    }

    public void Activate()
    {
        nick.text = player.name;
        nick.color = player.GetComponent<PhotonView>().IsMine ? minePlayerNickColor : otherPlayerNickColor;
    }
}
