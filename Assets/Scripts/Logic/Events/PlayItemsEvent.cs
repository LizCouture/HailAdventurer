using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemsEvent : GameEvent
{

    public PlayItemsEvent(bool timed, int duration = 0)
    {
        this.type = TimelineEventType.PlayItems;
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
