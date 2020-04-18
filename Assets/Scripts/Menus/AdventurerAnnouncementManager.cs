using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * AdventurerAnnouncementManager
 * 
 * This script controls the screen display of the Adventurer Announcement
 * It is told when to display and hide by the Event system.
 * 
 * It should be full screen during the AdventurerAnnouncementEvent and display
 *   a smaller version during the PlayItemsEvent, and be hidden at all other times.
 * 
 */
public class AdventurerAnnouncementManager : MonoBehaviour
{

    public GameObject largeAnnouncement;
    public GameObject smallAnnouncement;

    public Image largeAvatar;
    public Image smallAvatar;

    public TextMeshProUGUI largeNickname;
    public TextMeshProUGUI smallNickname;

    private CharacterAsset characterToDisplay;
    private string nickname;

    public static AdventurerAnnouncementManager Instance;

    public void loadAdventurerAnnouncementFromEvent()
    {
        AdventurerAnnouncementEvent thisRound;
        GameManager gm = GameManager.Instance;
        if (gm.currentEvent().type != TimelineEventType.AdventurerReveal)
        {
            Debug.LogError("In RoundAnnouncementManager but current event type is " + gm.currentEvent().type);
            return;
        }
        else
        {
            thisRound = gm.currentEvent() as AdventurerAnnouncementEvent;
            NetworkPlayer adventurer = gm.getPlayerByID(thisRound.playerIndex);
            populateFields(adventurer.avatar, adventurer.nickname);
        }
    }

        private void populateFields(CharacterAsset ca, string na)
    {
        characterToDisplay = ca;
        nickname = na;

        largeAvatar.sprite = characterToDisplay.AvatarImage;
        smallAvatar.sprite = characterToDisplay.AvatarImage;

        largeNickname.text = nickname;
        smallNickname.text = nickname;
    }

    public void startAnnouncement()
    {
        if (GameManager.Instance.currentEvent().type == TimelineEventType.AdventurerReveal)
        {
            loadAdventurerAnnouncementFromEvent();
            largeAnnouncement.SetActive(true);
            smallAnnouncement.SetActive(false);

            Debug.Log("Ending AdventurerReveal.");
            StartCoroutine(endAfterDuration(GameManager.Instance.currentEvent().duration*1.0f));
        }
        else
        {
            Debug.LogError("Attempt to revealAdventurerAnnouncement, but current event type is " +
                GameManager.Instance.currentEvent().type);
        }
    }

    public void endAnnouncement()
    {
        smallAnnouncement.SetActive(true);
        largeAnnouncement.SetActive(false);
    }

    public IEnumerator endAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        GameManager.Instance.endCurrentEvent();
    }

    private void Start()
    {
        Instance = this;
        smallAnnouncement.SetActive(false);
        largeAnnouncement.SetActive(false);
    }
}
