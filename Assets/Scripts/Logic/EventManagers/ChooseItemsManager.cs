using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseItemsManager : MonoBehaviour
{
    public CarouselSelector carousel;
    public Transform itemParent;
    public GameObject ItemPrefab;
    public List<Transform> Items;

    public GameObject chooseItemsAnnouncement;
    public Image avatar;
    public TextMeshProUGUI nicknameLabel;

    public List<NetworkPlayer> sellers;
    public NetworkPlayer buyer;

    private RopeTimer rt;
    private bool hitBuyIt;

    public static ChooseItemsManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        chooseItemsAnnouncement.SetActive(false);
        sellers = new List<NetworkPlayer>();
        Items = new List<Transform>();
        rt = GetComponent<RopeTimer>();
        hitBuyIt = false;
    }

    public void StartChoosing()
    {
        hitBuyIt = false;
        chooseItemsAnnouncement.SetActive(true);
        if (GameManager.Instance.currentEvent().type == TimelineEventType.ChooseItems)
        {
            GameEvent thisRound = GameManager.Instance.currentEvent();
            loadBuyingPlayerFromEvent();
            loadSellingPlayersFromGameManager();
            carousel.initializeItems(Items);

            Debug.Log("Ending event after duration: " + thisRound.duration * 1.0f);
            rt.SetTimer(duration: (float)thisRound.duration);
            rt.StartTimer();
        }
        else
        {
            Debug.LogError("Attempt to startSelling, but current event type is " +
                GameManager.Instance.currentEvent().type);
        }
    }

    private void loadBuyingPlayerFromEvent()
    {
        ChooseItemsEvent currentEvent = GameManager.Instance.currentEvent() as ChooseItemsEvent;
        buyer = GameManager.Instance.getPlayerByID(currentEvent.playerIndex);
        nicknameLabel.text = buyer.nickname;
        avatar.sprite = buyer.avatar.AvatarImage;
    }

    private void loadSellingPlayersFromGameManager()
    {
        GameManager gm = GameManager.Instance;
        int buyerID = (gm.currentEvent() as ChooseItemsEvent).playerIndex;
        for (int i = 0; i < gm.playerCount(); i++)
        {
            if (i!= buyerID)
            {
                sellers.Add(gm.getPlayerByID(i));
                addPlayedItemCardFromPlayer(gm.getPlayerByID(i));
            }
        }
    }

    private void addPlayedItemCardFromPlayer(NetworkPlayer np)
    {
        GameObject newPlayedItem = Instantiate(ItemPrefab, itemParent.transform.position, Quaternion.identity);
        newPlayedItem.transform.parent = itemParent.transform;
        PlayedItem script = newPlayedItem.GetComponent<PlayedItem>();
        script.LoadFromNetworkPlayer(np);
        Items.Add(newPlayedItem.transform);
    }

    // In the future we might want locking in a choice and allowing time to expire to return different options.
    // For now, we just want to make sure that it doesn't fire twice.
    public void LockItIn()
    {
        hitBuyIt = true;
        int winner = carousel.GetSelection();
        sellers[winner].giveCoin();
        StartCoroutine(GameManager.Instance.endCurrentEventAfterDuration(1.0f));
    }

    public void TimesUp()
    {
        if (!hitBuyIt)
        {
            int winner = carousel.GetSelection();
            sellers[winner].giveCoin();
            StartCoroutine(GameManager.Instance.endCurrentEventAfterDuration(1.0f));
        }
    }

    public void CleanUp()
    {
        sellers = new List<NetworkPlayer>();
        chooseItemsAnnouncement.SetActive(false);
    }
}
