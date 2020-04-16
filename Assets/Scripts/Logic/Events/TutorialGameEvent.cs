using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using System;

public class TutorialGameEvent : GameEvent
{
    public string viewCat = "Gameplay";
    public string viewName = "Tutorial01";

    public TutorialGameEvent(bool timed, int duration = 0)
    {
        this.type = TimelineEventType.Tutorial;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        Debug.Log("Running Tutorial");
        UIView.ShowView(viewCat, viewName);
       
    }

    public override void onEnd()
    {
        UIView.HideView(viewCat, viewName);
        Debug.Log("Done With Tutorial");
        base.onEnd();
    }
}
