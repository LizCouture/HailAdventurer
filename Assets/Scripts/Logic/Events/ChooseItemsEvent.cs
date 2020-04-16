using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseItemsEvent : GameEvent
{
    public int playerIndex;

    public ChooseItemsEvent(bool timed, int playerIndex, int duration = 0)
    {
        this.type = TimelineEventType.ChooseItems;
        this.timed = timed;
        this.duration = duration;
        this.playerIndex = playerIndex;
    }

    public override void onStart()
    {
        base.onStart();
        Debug.Log(GameManager.Instance.getPlayerByID(playerIndex).nickname + " will pick her favorite!");
    }

    public override void onEnd()
    {
        Debug.Log("Playing items ends!");
        base.onEnd();
    }
}
