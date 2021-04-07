using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> players;

    void Start()
    {
        var obj = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in obj)
        {
            players.Add(player.GetComponent<Player>());
        }
    }
    public enum Players
    {
        First, Second, Third, Fourth
    }

    public Player FindPlayer(GameObject sender)
    {
        foreach (Player player in players)
        {
            if (sender.GetComponent<owner>().own == player.title)
            {
                return player;
            }
        }
        return null;
    }
}
