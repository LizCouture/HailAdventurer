using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundAnnouncementManager : MonoBehaviour
{
    public TextMeshProUGUI roundNumber;

    public void loadRoundAnnouncementFromEvent()
    {
        RoundAnnouncementEvent thisRound;
        GameManager gm = GameManager.Instance;
        if (gm.currentEvent().type != TimelineEventType.RoundAnnouncement)
        {
            Debug.LogError("In RoundAnnouncementManager but current event type is " + gm.currentEvent().type);
            return;
        } else
        {
            thisRound = gm.currentEvent() as RoundAnnouncementEvent;
        }
        roundNumber.text = (thisRound.roundNum + 1).ToString();
    }
}
