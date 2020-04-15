using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<NetworkPlayer> Players;

    public int localPlayer;

    public LobbyManager lobbyManager;
    public AvatarManager avatarManager;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Players = new List<NetworkPlayer>();
        DontDestroyOnLoad(gameObject);
    }

    public void initializeGame()
    {
        Debug.Log("Initializing Game");
        Players = new List<NetworkPlayer>();
        Debug.Log("Players.Count: " + Players.Count);
        localPlayer = addPlayer(new NetworkPlayer());

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
}
