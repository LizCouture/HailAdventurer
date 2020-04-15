using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAnnouncementEvent : GameEvent
{
    public int playerIndex;

    public AdventurerAnnouncementEvent(GameTimeline tl, bool timed, int playerIndex, int duration=0)
    {
        myTimeline = tl;
        this.type = TimelineEventType.AdventurerReveal;
        this.timed = timed;
        this.duration = duration;
        this.playerIndex = playerIndex;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log("Player " + GameManager.Instance.getPlayerByID(playerIndex).nickname + " is coming to town!");
    }

    public override void onEnd()
    {
        base.onEnd();
        Debug.Log("Sell sell sell!!!");
    }
}
