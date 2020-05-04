using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<NetworkPlayer> Players;

    public int localPlayer;
    public int currentAdventurer;

    public LobbyManager lobbyManager;
    public AvatarManager avatarManager;
    private GameTimeline timeline;

    public Deck adventurerDeck;
    public Deck adventurerDiscard;
    public Deck itemDeck;
    public Deck itemDiscard;

    public CardLogic currentAdventurerCard;

    public int HANDLIMIT = 8;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Players = new List<NetworkPlayer>();
        currentAdventurer = 0;
        currentAdventurerCard = null;
        DontDestroyOnLoad(gameObject);
    }

    public void initializeGame()
    {
        Debug.Log("Initializing Game");
        Players = new List<NetworkPlayer>();
        Debug.Log("Players.Count: " + Players.Count);
        localPlayer = addPlayer(new NetworkPlayer());

        Debug.Log("creating decks");
        adventurerDiscard = new Deck();
        itemDiscard = new Deck();
        adventurerDeck = new Deck("SO Assets/Cards/Adventurers");
        itemDeck = new Deck("SO Assets/Cards/Items");

        adventurerDeck.Shuffle();
        itemDeck.Shuffle();

        currentAdventurerCard = null;
        Debug.Log("adventurerDiscard " + adventurerDiscard.ToString());
        Debug.Log("adventurerDeck: " + adventurerDeck.ToString());

        avatarManager.ShuffleDictionary();
        lobbyManager.initializeLobby();
    }

    // Creates a new player and returns the id of the player generated.
    public int addPlayer(NetworkPlayer newPlayer)
    {

        newPlayer.coins = 0;
        newPlayer.creatingAvatar = true;
        newPlayer.isConnected = false;
        Players.Add(newPlayer);
        lobbyManager.updateSlotPlayers();
        return Players.Count - 1;
    }
    public void addAIPlayer()
    {
        NetworkPlayer ai = new NetworkPlayer();
        ai.coins = 0;
        ai.creatingAvatar = true;
        ai.isConnected = false;
        ai.isAI = true;
        
        string randomName = "";
        string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int i = 0; i < 7; i++)
        {
            randomName += glyphs[Random.Range(0, glyphs.Length)];
        }
        ai.nickname = randomName;
        ai.avatar = avatarManager.DealAvatar();
        Players.Add(ai);
        lobbyManager.updateSlotPlayers();
        connectPlayer(Players.Count - 1);
    }

    public void removePlayer(NetworkPlayer playerToRemove)
    {
        int removeIndex = Players.IndexOf(playerToRemove);
        Players.Remove(playerToRemove);
        lobbyManager.updateSlotPlayers();
    }

    public void connectPlayer(int newPlayer)
    {
        if (newPlayer < 0 || newPlayer >= Players.Count)
        {
            Debug.LogError("Tried to connect unrecognized player: " + newPlayer.ToString());
            return;
        }
        if (Players[newPlayer].avatar == null)
        {
            Debug.Log("Tried to create player without avatar.  Randomizing.");
            Players[newPlayer].avatar = avatarManager.DealAvatar();
        }
        if (Players[newPlayer].nickname == null)
        {
            Debug.Log("Tried to create player without name.  Randomizing.");
            Players[newPlayer].nickname = "NoNamePlayer";
        }
        Players[newPlayer].creatingAvatar = false;
        Players[newPlayer].isConnected = true;
        lobbyManager.updateSlotPlayers();
    }

    public NetworkPlayer getPlayerByID(int id)
    {
        if (id < 0 || id >= Players.Count)
        {
            Debug.LogError("getPlayerByID called with invalid id: " + id);
        }
        return Players[id];
    }

    public int playerCount()
    {
        return Players.Count;
    }

    public void startGame()
    {
        timeline = new GameTimeline(playerCount());
        Debug.Log("timeLine: " + timeline.ToString());
        goToNextEvent();
    }

    public void goToNextEvent()
    {
        Debug.Log("goToNextEvent");
        if (timeline.queueLength() > 0)
        {
            timeline.nextInQueue();
        }
        else Debug.Log("Que complete");
    }

    public GameEvent currentEvent()
    {
        return timeline.currentEvent;
    }

    public void endCurrentEvent()
    {
        Debug.Log("GameManager: endCurrentEvent");
        timeline.endCurrentEvent();
    }
    public IEnumerator endCurrentEventAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        endCurrentEvent();
    }

    public void dealNewAdventurer()
    {
        if (currentAdventurerCard != null && currentAdventurerCard.ca != null)
        {
            Debug.Log("dealNewAdventurer adventurerDiscard: " + adventurerDiscard.ToString());
            Debug.Log("currentAdventurer: " + currentAdventurerCard.ToString());
            Debug.Log("currentAdventurer.ca : " + currentAdventurerCard.ca.ToString());
            adventurerDiscard.returnToDeck(currentAdventurerCard.ca);
        }

        currentAdventurerCard = adventurerDeck.DealCard();
    }

    public Deck ItemDeck()
    {
        return itemDeck;
    }

    public void DiscardItem(CardLogic item)
    {
        itemDiscard.returnToDeck(item.ca);
    }

    public void playItemsForAI()
    {
        for(int i = 0; i < Players.Count; i++)
        {
            if (Players[i].isAI && (i != currentAdventurer))
            {
                List<CardLogic> cardsToPlay = new List<CardLogic>();
                for (int j = 0; j < 2; j++)
                {
                    cardsToPlay.Add(itemDeck.DealCard());
                }
            }
        }
    }
}
