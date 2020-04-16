using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportAdventureWinnerEvent : GameEvent
{
    public ReportAdventureWinnerEvent(bool timed, int duration = 0)
    {
        this.type = TimelineEventType.ReportAdventureWinner;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        Debug.Log("We have a winner!");
        base.onStart();
    }

    public override void onEnd()
    {
        Debug.Log("We're done having a winner!");
        base.onEnd();
    }
}
