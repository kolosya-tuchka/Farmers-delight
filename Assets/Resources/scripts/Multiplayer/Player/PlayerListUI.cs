using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private List<PlayerCardUI> playerCards;

    public void UpdatePlayerCards()
    {
        foreach (var card in playerCards)
        {
            card.gameObject.SetActive(false);
        }

        for (int i = 0; i < MPManager.players.Count; ++i)
        {
            AddPlayerCard(i);
        }
    }

    void AddPlayerCard(int index)
    {
        playerCards[index].gameObject.SetActive(true);
        playerCards[index].player = MPManager.players[index].GetComponent<Player>();
        playerCards[index].Activate();
    }
}
