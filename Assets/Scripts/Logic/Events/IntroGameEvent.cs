using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroGameEvent : GameEvent
{

    public IntroGameEvent(GameTimeline tl, bool timed, int duration = 0)
    {
        myTimeline = tl;
        this.type = TimelineEventType.Intro;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log("Running Intro");
    }

    public override void onEnd()
    {
        base.onEnd();
        Debug.Log("Done With Intro");
    }
}
