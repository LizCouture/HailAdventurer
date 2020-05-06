using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer 
{
    public CharacterAsset avatar;
    public string nickname;
    public Hand hand;
    public List<CardLogic> cardsPlayed;
    public int coins;

    public bool isConnected;
    public bool creatingAvatar;
    public bool isAI;

    public NetworkPlayer()
    {
        hand = new Hand();
        cardsPlayed = new List<CardLogic>();
    }

    public void addCardToHand(CardLogic card)
    {
        hand.addCard(card);
    }

    public void removeCardFromHand(CardLogic card)
    {
        hand.removeCard(card);
    }

    public int cardsInHand()
    {
        return hand.cardCount();
    }

    public List<CardLogic> CardsPlayed()
    {
        return cardsPlayed;
    }

    public void PlayCards(List<CardLogic> newCards)
    {
        Debug.Log("in PlayCards.");
        if (newCards.Count != 2)
        {
            Debug.LogError("ERROR:  NetworkPlayer tried to PlayCards with " + newCards.Count + " cards");
            //TODO:  Make this fix the problem.
        } else
        {
            if (cardsPlayed != null && cardsPlayed.Count > 0)
            {
                foreach (CardLogic card in cardsPlayed)
                {
                    GameManager.Instance.DiscardItem(card);
                    Debug.Log("Discarding Card: " + card.ToString());
                    cardsPlayed.Remove(card);
                }
            }
            foreach(CardLogic card in newCards)
            {
                cardsPlayed.Add(card);
            }
            if (cardsPlayed.Count != 2)
            {
                Debug.LogError("ERROR:  After PlayCards, list of cardsPlayed.Count = " + cardsPlayed.Count);
            }
        }
    }
}
