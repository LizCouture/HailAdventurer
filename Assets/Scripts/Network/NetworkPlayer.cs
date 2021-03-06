﻿using System.Collections;
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

    public void giveCoin()
    {
        coins++;
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
                while (cardsPlayed.Count > 0)
                {
                    CardLogic card = cardsPlayed[0];
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

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        NetworkPlayer other = obj as NetworkPlayer;
        if (other.nickname == nickname && other.avatar == avatar)
        {
            return true;
        }
        return false;
    }
}
