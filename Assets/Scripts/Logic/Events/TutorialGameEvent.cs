using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameEvent : GameEvent
{
    public TutorialGameEvent(GameTimeline tl, bool timed, int duration = 0)
    {
        this.type = TimelineEventType.Tutorial;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log("Running Tutorial");
    }

    public override void onEnd()
    {
        base.onEnd();
        Debug.Log("Done With Tutorial");
    }
}
