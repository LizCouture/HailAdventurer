using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundAnnouncementEvent : GameEvent
{
    public int roundNum;

    public RoundAnnouncementEvent(GameTimeline tl, bool timed, int roundNum, int duration = 0)
    {
        myTimeline = tl;
        this.type = TimelineEventType.RoundAnnouncement;
        this.timed = timed;
        this.duration = duration;
        this.roundNum = roundNum;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log("ANNOUNCING ROUND " + roundNum.ToString());
    }

    public override void onEnd()
    {
        Debug.Log("ENDING ROUND " + roundNum.ToString() + " ANNOUNCEMENT");
        base.onEnd();
    }
}
