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
}
