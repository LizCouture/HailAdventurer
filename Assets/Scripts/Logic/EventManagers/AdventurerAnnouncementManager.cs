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

    public GameObject largeCard;
    public GameObject smallCard;

    public TextMeshProUGUI commandText;

    private CharacterAsset characterToDisplay;
    private string nickname;
    private CardAsset cardToDisplay;

    public static AdventurerAnnouncementManager Instance;

    public void startAnnouncement(CardLogic adventurerCard)
    {
        if (GameManager.Instance.currentEvent().type == TimelineEventType.AdventurerReveal)
        {
            loadAdventurerAnnouncementFromEvent();
            loadAdventurerCardsFromAsset(adventurerCard.ca);

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
        commandText.text = "Tell us about \n yourself!";
    }

    public void endAnnouncement()
    {
        // If the local player is the adventurer, keep this screen active during the next event.
        // Otherwise, show the smaller version of the announcement.

        AdventurerAnnouncementEvent thisRound;
        GameManager gm = GameManager.Instance;
        if (gm.currentEvent().type != TimelineEventType.AdventurerReveal)
        {
                Debug.Log ("We're trying to end the AdventurerAnnouncement, but the current event type is " + 
                gm.currentEvent().ToString());
        } else
        {
            thisRound = gm.currentEvent() as AdventurerAnnouncementEvent;
            if (thisRound.playerIndex == gm.localPlayer)
            {
                largeAnnouncement.SetActive(true);
                smallAnnouncement.SetActive(false);
                commandText.text = "Wait a sec \n while everybody \n picks some stuff \n to sell you!";
            } else
            {
                smallAnnouncement.SetActive(true);
                largeAnnouncement.SetActive(false);
            }
        }
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

    private void loadAdventurerCardsFromAsset(CardAsset ca)
    {
        OneCardManager smallManager = smallCard.GetComponent<OneCardManager>();
        OneCardManager largeManager = largeCard.GetComponent<OneCardManager>();
        smallManager.cardAsset = ca;
        largeManager.cardAsset = ca;
        smallManager.ReadCardFromAsset();
        largeManager.ReadCardFromAsset();
    }
}
