using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemsManager : MonoBehaviour
{

    public GameObject TimerContainer;
    public static PlayItemsManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public void setupPlayItemsScene()
    {
        PlayItemsEvent thisRound;
        GameManager gm = GameManager.Instance;
        if (gm.currentEvent().type != TimelineEventType.PlayItems)
        {
            Debug.LogError("In PlayItemsManager but current event type is " + gm.currentEvent().type);
            return;
        }
        else
        {
            thisRound = gm.currentEvent() as PlayItemsEvent;
        }

        // Start the timer.
        RopeTimer timerScript = TimerContainer.GetComponent<RopeTimer>();
        timerScript.SetTimer(duration: (float)thisRound.duration);
    }

    public void TimesUp()
    {
        // TODO: Implement "locking in cards"
        List<CardLogic> selectedCards = new List<CardLogic>();
        Table t = Table.Instance;
        if(t.SlotLeft == null)
        {
            Slot leftSlot = TableVisual.Instance.leftSlot.GetComponent<Slot>();
            CardLogic newCard = GameManager.Instance.itemDeck.DealCard();
            new PlayItemFromDeckCommand(newCard, Player.Instance, true, leftSlot).AddToQueue();
        }

        // If either slot is empty, deal a random card from the deck into that slot.
        // TODO:  Animate that.  Make it a coroutine and ensure it finishes executing

        //Clear Slots.

        //End Event.
    }
}
