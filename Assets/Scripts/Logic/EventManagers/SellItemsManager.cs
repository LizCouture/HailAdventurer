using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemsManager : MonoBehaviour
{
    public GameObject sellItemsAnnouncement;
    public Image avatar;
    public TextMeshProUGUI nicknameLabel;

    public GameObject leftCard;
    public GameObject rightCard;

    public TextMeshProUGUI commandText;

    private NetworkPlayer salesman;
    private CharacterAsset characterToDisplay;
    private string nickname;
    private CardAsset cardToDisplayLeft;
    private CardAsset cardToDisplayRight;

    public static SellItemsManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        sellItemsAnnouncement.SetActive(false);
    }

    public void startSelling(int sellerIndex) {

        sellItemsAnnouncement.SetActive(true);
        if (GameManager.Instance.currentEvent().type == TimelineEventType.SellItems)
        {
            loadSellingPlayerFromEvent();

            if (salesman.cardsPlayed == null || salesman.cardsPlayed.Count < 2)
            {
                Debug.Log("ERROR.  Can not find salseman's cards played.  TODO: Handle this better.");
            }
            else {
                loadCardFromEvent(leftCard, salesman.cardsPlayed[0]);
                loadCardFromEvent(rightCard, salesman.cardsPlayed[1]);
            }

            Debug.Log("Ending event after duration: " + GameManager.Instance.currentEvent().duration * 1.0f);
            StartCoroutine(GameManager.Instance.endCurrentEventAfterDuration(GameManager.Instance.currentEvent().duration * 1.0f));
        }
        else
        {
            Debug.LogError("Attempt to startSelling, but current event type is " +
                GameManager.Instance.currentEvent().type);
        }
    }

    private void loadSellingPlayerFromEvent()
    {
        SellItemsEvent thisRound;
        GameManager gm = GameManager.Instance;
        if (gm.currentEvent().type != TimelineEventType.SellItems)
        {
            Debug.LogError("In SellItemsManager but current event type is " + gm.currentEvent().type);
            return;
        }
        else
        {
            thisRound = gm.currentEvent() as SellItemsEvent;
            salesman = gm.getPlayerByID(thisRound.playerIndex);
            populateFields(salesman.avatar, salesman.nickname);
        }

    }
    public void endAnnouncement()
    {
        sellItemsAnnouncement.SetActive(false);
       
    }

    private void populateFields(CharacterAsset ca, string na)
    {
        characterToDisplay = ca;
        nickname = na;

        avatar.sprite = characterToDisplay.AvatarImage;
        
        nicknameLabel.text = nickname;
    }

    private void loadCardFromEvent(GameObject card, CardLogic cl)
    {
       OneCardManager cardManager = card.GetComponent<OneCardManager>();
       cardManager.cardAsset = cl.ca;
       cardManager.ReadCardFromAsset();
    }
}
