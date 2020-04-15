using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItemsEvent : GameEvent
{
    public int playerIndex;
    public SellItemsEvent(GameTimeline tl, bool timed, int playerIndex, int duration = 0)
    {
        myTimeline = tl;
        this.type = TimelineEventType.SellItems;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log("Playing items begins!");
    }

    public override void onEnd()
    {
        Debug.Log("Playing items ends!");
        base.onEnd();
    }
}
