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
        timerScript.StartTimer();
    }

    public void TimesUp()
    {
        Debug.Log("times Up!");
        // TODO: Implement "locking in cards"
        List<CardLogic> selectedCards = new List<CardLogic>();
        Table t = Table.Instance;
        if(t.SlotLeft == null)
        {
            Debug.Log("t.SlotLeft == null");
            Slot leftSlot = TableVisual.Instance.leftSlot.GetComponent<Slot>();
            CardLogic newCard = GameManager.Instance.itemDeck.DealCard();
            new PlayItemFromDeckCommand(newCard, Player.Instance, true, leftSlot).AddToQueue();
            t.SlotLeft = newCard;
        }
        if(t.SlotRight == null)
        {
            Debug.Log("t.SlotRight == null");
            Slot rightSlot = TableVisual.Instance.rightSlot.GetComponent<Slot>();
            CardLogic newCard = GameManager.Instance.itemDeck.DealCard();
            new PlayItemFromDeckCommand(newCard, Player.Instance, true, rightSlot).AddToQueue();
            t.SlotRight = newCard;
        }

        selectedCards.Add(t.SlotLeft);
        selectedCards.Add(t.SlotRight);

        GameManager.Instance.getPlayerByID(GameManager.Instance.localPlayer).PlayCards(selectedCards);
        GameManager.Instance.playItemsForAI();
        StartCoroutine(GameManager.Instance.endCurrentEventAfterDuration(5.0f));
        //Clear Slots.

        //End Event.
    }

    public void cleanUp()
    {
        TableVisual.Instance.CleanUpSlots();
    }
}
