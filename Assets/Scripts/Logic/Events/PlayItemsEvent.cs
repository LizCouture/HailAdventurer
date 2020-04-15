using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemsEvent : GameEvent
{

    public PlayItemsEvent(GameTimeline tl, bool timed, int duration = 0)
    {
        myTimeline = tl;
        this.type = TimelineEventType.PlayItems;
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
