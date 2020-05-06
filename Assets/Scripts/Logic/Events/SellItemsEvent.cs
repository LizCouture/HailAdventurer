using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItemsEvent : GameEvent
{
    public int playerIndex;
    public SellItemsEvent( bool timed, int playerIndex, int duration = 0)
    {
        this.type = TimelineEventType.SellItems;
        this.timed = timed;
        this.duration = duration;
        this.playerIndex = playerIndex;
    }

    public override void onStart()
    {
        GameManager gm = GameManager.Instance;
        gm.currentSalesman = playerIndex;
        Debug.Log("Player " + gm.getPlayerByID(playerIndex).nickname + " is selling items!");

        // Draw an adventure card, and display it.
        //base.onStart();
        SellItemsManager.Instance.startSelling(playerIndex);
    }

    public override void onEnd()
    {
        SellItemsManager.Instance.endAnnouncement();
        base.onEnd();
    }
}
