﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck{

    private List<CardAsset> cards;


    public Deck()
    {
        Debug.Log("Creating Deck empty");
        cards = new List<CardAsset>();
    }

    // To create a deck from a folder of scripted objects
    public Deck(string resourceLocation)
    {
        cards = new List<CardAsset>(Resources.LoadAll<CardAsset>(resourceLocation));
    }

    void Awake()
    {
        cards.Shuffle();
    }

    public CardLogic DealCard()
    {
        CardLogic newCard = new CardLogic(cards[0]);
        cards.RemoveAt(0);
        return newCard;
    }

    public void returnToDeck(CardAsset card)
    {
        cards.Add(card);
    }

    public int cardCount()
    {
        return cards.Count;
    }

    public void Shuffle()
    {
        cards.Shuffle();
    }

    public override string ToString()
    {
        string deckstring = "";
        foreach (CardAsset card in cards){
            deckstring += card.CardImage.name;
        }
        return deckstring;
    }
	
}
