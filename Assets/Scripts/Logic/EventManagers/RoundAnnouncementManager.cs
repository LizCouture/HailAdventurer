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

    public void endAnnouncement()
    {
        if (GameManager.Instance.currentEvent().type == TimelineEventType.RoundAnnouncement)
        {
            Debug.Log("Ending Tutorial.");
            StartCoroutine(endAfterDuration(3.0f));
        }
        else
        {
            Debug.LogError("Attempt to end rountAnnouncement, but current event type is " + 
                GameManager.Instance.currentEvent().type);
        }
    }


    public IEnumerator endAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameManager.Instance.endCurrentEvent();
    }
}
