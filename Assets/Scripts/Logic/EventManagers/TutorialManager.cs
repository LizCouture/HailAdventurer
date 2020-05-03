using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public void endTutorial()
    {
        if (GameManager.Instance.currentEvent().type == TimelineEventType.Tutorial)
        {
            Debug.Log("Ending Tutorial.");
            StartCoroutine(endAfterDuration(3.0f));
        }
        else
        {
            Debug.LogError("Attempt to end tutorial, but tutorial is not current event.");
        }
    }


    public IEnumerator endAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameManager.Instance.endCurrentEvent();
    }
}
