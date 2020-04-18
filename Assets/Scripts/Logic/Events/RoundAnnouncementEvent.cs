using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using System.Threading.Tasks;

public class RoundAnnouncementEvent : GameEvent
{
    public int roundNum;

    public string viewCat = "Gameplay";
    public string viewName = "RoundAnnouncement";

    public RoundAnnouncementEvent(bool timed, int roundNum, int duration = 0)
    {
        this.type = TimelineEventType.RoundAnnouncement;
        this.timed = timed;
        this.duration = duration;
        this.roundNum = roundNum;
    }

    public override void onStart()
    {
        Debug.Log("ANNOUNCING ROUND " + roundNum.ToString());
        UIView.ShowView(viewCat, viewName);
        //base.onStart();
    }

    public override void onEnd()
    {
        Debug.Log("ENDING ROUND " + roundNum.ToString() + " ANNOUNCEMENT");
        UIView.HideView(viewCat, viewName);
        base.onEnd();
    }
}
