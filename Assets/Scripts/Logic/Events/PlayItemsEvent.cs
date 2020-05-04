using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemsEvent : GameEvent
{

    public PlayItemsEvent(bool timed, int duration = 0)
    {
        this.type = TimelineEventType.PlayItems;
        this.timed = timed;
        this.duration = duration;
    }

    public override void onStart()
    {
        Debug.Log("Playing items begins!");
        GameManager gm = GameManager.Instance;
        Player p = Player.Instance;
        while(gm.getPlayerByID(gm.localPlayer).cardsInHand() < gm.HANDLIMIT)
        {
            Debug.Log("Drawing a card");
            p.DrawACard();
            //TODO: Deal Card To Player
        }
        PlayItemsManager.Instance.setupPlayItemsScene();

    }

    public override void onEnd()
    {
        Debug.Log("Playing items ends!");
        PlayItemsManager.Instance.cleanUp();
        base.onEnd();
    }
}
