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
        Debug.Log("Player " + GameManager.Instance.getPlayerByID(playerIndex).nickname + " is coming to town!");
        //base.onStart();
        AdventurerAnnouncementManager.Instance.startAnnouncement();
    }

    public override void onEnd()
    {
        Debug.Log("Sell sell sell!!!");
        AdventurerAnnouncementManager.Instance.endAnnouncement();
        base.onEnd();
    }
}
