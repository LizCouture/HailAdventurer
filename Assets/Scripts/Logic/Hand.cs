using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand 
{
    private List<CardLogic> CardsInHand = new List<CardLogic>(); 

    public Hand()
    {
        CardsInHand = new List<CardLogic>();
    }

    public void addCard(CardLogic card, int index=0)
    {
        CardsInHand.Insert(index, card); 
    }

    public void removeCard(int index)
    {
        CardsInHand.RemoveAt(index);
    }

    public void removeCard(CardLogic card)
    {
        CardsInHand.Remove(card);
    }

    public int cardCount()
    {
        return CardsInHand.Count;
    }

    public CardLogic cardAtIndex(int index)
    {
        return CardsInHand[index];
    }

}
