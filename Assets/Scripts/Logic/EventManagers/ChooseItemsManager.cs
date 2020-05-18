using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Profiling.Memory.Experimental;

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
    public NetworkPlayer winner;

    private RopeTimer rt;
    private bool hitBuyIt;

    public GameObject PlayerPanel;
    public GameObject PlayerInfoPrefab;
    public List<PlayerInfoView> playerInfoSpots;

    public static ChooseItemsManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        chooseItemsAnnouncement.SetActive(false);
        CleanUp();
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
        Debug.Log("Lock It In.");
        hitBuyIt = true;
        StartCoroutine( EndIt());
    }

    public void TimesUp()
    {
        Debug.Log("TimesUp");
        if (!hitBuyIt)
        {
            StartCoroutine( EndIt());
        }
    }

    public IEnumerator EndIt()
    {
        Debug.Log("EndIt");
        int winningIndex = carousel.GetSelection();
        Debug.Log("Winning Index: " + winningIndex);
        PlayedItem winnerView = carousel.Items[winningIndex].GetComponent<PlayedItem>();
        Debug.Log("WinnerView: " + winnerView);
        winner = winnerView.np;
        Debug.Log("winner: " + winner);
        populatePlayerList();
        yield return StartCoroutine(showAwardingCoins());
        StartCoroutine(GameManager.Instance.endCurrentEventAfterDuration(1.0f));
    }

    public void CleanUp()
    {
        sellers = new List<NetworkPlayer>();

        Items = new List<Transform>();
        rt = GetComponent<RopeTimer>();
        hitBuyIt = false;
        playerInfoSpots = new List<PlayerInfoView>();

        foreach (Transform child in PlayerPanel.transform) {
            Destroy(child.gameObject);
        }

        carousel.cleanupCarousel();

        chooseItemsAnnouncement.SetActive(false);
    }

    public void populatePlayerList()
    {
        for (int i = 0; i < GameManager.Instance.playerCount(); i++)
        {
            GameObject pi = Instantiate(PlayerInfoPrefab);
            pi.transform.SetParent(PlayerPanel.transform);
            PlayerInfoView infoView = pi.GetComponent<PlayerInfoView>();
            playerInfoSpots.Add(infoView);
            infoView.loadFromPlayer(GameManager.Instance.getPlayerByID(i));
        }
    }

    public IEnumerator showAwardingCoins()
    {
        Debug.Log("showAwardingCoins");
        Animator anim = PlayerPanel.GetComponent<Animator>();
        anim.SetInteger("numItems", GameManager.Instance.playerCount());
        anim.SetBool("open", true);
        yield return new WaitForSeconds(1);
        // Animate coin to player.
        Debug.Log("playerInfoSpots: " + playerInfoSpots + " winner: " + winner);
        for(int i = 0; i < playerInfoSpots.Count; i++)
        {
            if (winner == playerInfoSpots[i].np)
            {
                yield return StartCoroutine(playerInfoSpots[i].giveCoin());
                winner.giveCoin();
            }
        }
        // Wait a second
        yield return new WaitForSeconds(3);
        // Close panel
        anim.SetBool("open", false);

    }
}
