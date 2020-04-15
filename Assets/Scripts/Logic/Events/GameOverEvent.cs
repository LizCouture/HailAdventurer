using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvent : GameEvent
{
    public GameOverEvent(GameTimeline tl, bool timed, int duration = 0)
    {
        this.myTimeline = tl;
        this.type = TimelineEventType.GameOver;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log("Game over!");
    }

    public override void onEnd()
    {
        Debug.Log("No really we're done!");
        base.onEnd();
    }
}
