using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEvent : GameEvent
{
    public GameOverEvent(bool timed, int duration = 0)
    {
        this.type = TimelineEventType.GameOver;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        Debug.Log("Game over!");
        base.onStart();
    }

    public override void onEnd()
    {
        Debug.Log("No really we're done!");
        base.onEnd();
    }
}
