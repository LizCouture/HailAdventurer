using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doozy.Engine.UI;

public class IntroGameEvent : GameEvent
{
    public string viewCat = "Gameplay";
    public string viewName = "Intro01";

    public IntroGameEvent(bool timed, int duration = 0)
    {
        this.type = TimelineEventType.Intro;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        Debug.Log("Running Intro");
        SceneManager.LoadScene("InGame", LoadSceneMode.Single);
        // Relying on IntroManager to load things in, because it starts automatically when the scene loads.
        //base.onStart();
        //UIView.ShowView(viewCat, viewName);
    }

    public override void onEnd()
    {
        Debug.Log("Done With Intro");
        //UIView.HideView(viewCat, viewName);
        base.onEnd();
    }
}
