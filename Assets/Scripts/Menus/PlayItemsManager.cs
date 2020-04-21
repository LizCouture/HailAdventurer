using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemsManager : MonoBehaviour
{
    public void setupPlayItemsScene()
    {
        PlayItemsEvent thisRound;
        GameManager gm = GameManager.Instance;
        if (gm.currentEvent().type != TimelineEventType.PlayItems)
        {
            Debug.LogError("In PlayItemsManager but current event type is " + gm.currentEvent().type);
            return;
        }
        else
        {
            thisRound = gm.currentEvent() as PlayItemsEvent;
        }
        
        // Ensure all players have 8 cards.
    }
}
