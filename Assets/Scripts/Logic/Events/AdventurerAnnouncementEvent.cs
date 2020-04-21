using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAnnouncementEvent : GameEvent
{
    public int playerIndex;

    public AdventurerAnnouncementEvent(bool timed, int playerIndex, int duration=0)
    {
        this.type = TimelineEventType.AdventurerReveal;
        this.timed = timed;
        this.duration = duration;
        this.playerIndex = playerIndex;
    }

    public override void onStart()
    {
        GameManager gm = GameManager.Instance;
        Debug.Log("Player " + gm.getPlayerByID(playerIndex).nickname + " is coming to town!");

        // Draw an adventure card, and display it.
        gm.dealNewAdventurer();
        //base.onStart();
        AdventurerAnnouncementManager.Instance.startAnnouncement(gm.currentAdventurerCard);
    }

    public override void onEnd()
    {
        Debug.Log("Sell sell sell!!!");
        AdventurerAnnouncementManager.Instance.endAnnouncement();
        base.onEnd();
    }
}
