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
    }

    public override void onStart()
    {
        Debug.Log("Playing items begins!");
        base.onStart();
    }

    public override void onEnd()
    {
        Debug.Log("Playing items ends!");
        base.onEnd();
    }
}
