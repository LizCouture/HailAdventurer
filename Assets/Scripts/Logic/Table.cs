using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour 
{
    // This used to be used for Creatures on the table.  We need to use it for items suggested.  
    public CardLogic SlotLeft;
    public CardLogic SlotRight;
    public static Table Instance;

    public void PlaceCardAt(int index, CardLogic card)
    {
        if (index == 0)
        {
            SlotLeft = card;
        } else if (index == 1)
        {
            SlotRight = card;
        } else
        {
            Debug.LogError("Tried to insert card at bad index: " + index);
        }
    }

    private void Awake()
    {
        SlotLeft = null;
        SlotRight = null;
        Instance = this;
    }

    public void RemoveCardFrom(int index)
    {
        CardLogic removedCard;
        if (index == 0)
        {
            removedCard = SlotLeft;
            SlotLeft = null;
        } else if (index == 1)
        {
            removedCard = SlotRight;
            SlotRight = null;
        } else
        {
            Debug.LogError("Tried to remove card at bad index : " + index);
        }
    }

    public void ReturnCardToHandFrom(int index, GameObject card, Player p)
    {
        CardLogic cl;
        if(p == null)
        {
            p = Player.Players[0];
        }
        if (index == 0)
        {
            cl = SlotLeft;
            new TakeCardBackCommand(cl, card, p).AddToQueue();
            SlotLeft = null;
        } else if (index == 1)
        {
            cl = SlotRight;
            new TakeCardBackCommand(cl, card, p).AddToQueue();
            SlotRight = null;
        } else
        {
            Debug.LogError("ReturnCardToHandFrom called with out of bounds index: " + index);
            return;
        }
        
    }
}
