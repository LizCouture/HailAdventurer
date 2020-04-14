using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

    public List<CardAsset> cards = new List<CardAsset>();

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
	
}
