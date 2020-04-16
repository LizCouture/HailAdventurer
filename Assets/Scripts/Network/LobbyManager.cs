using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI roomNameLabel;
    public PlayerSlot[] playerSlots;

    public NetworkPlayer localPlayer;

    [SerializeField]
    protected string roomName;

    const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const int roomNameChars = 5;

    public GameObject popup;

    // Start is called before the first frame update
    void Awake()
    {
        randomizeRoomname();
    }

    public void initializeLobby()
    {
        Debug.Log("initializing lobby");
        randomizeRoomname();
        promptCharacterCreation();
    }
    private void randomizeRoomname()
    {
        string randomName = "";
        for (int i = 0; i < roomNameChars; i++)
        {
            randomName += glyphs[Random.Range(0, glyphs.Length)];
        }
        roomName = randomName;
        roomNameLabel.text = "Room: " + randomName;
    }

    private void promptCharacterCreation()
    {
        popup.SetActive(true);
        CreateCharacterMenu ccm = popup.GetComponent<CreateCharacterMenu>();
        ccm.OpenMenu(GameManager.Instance.localPlayer);
    }

    public void updateSlotPlayers()
    {
        GameManager gm = GameManager.Instance;
        if(gm.playerCount() > playerSlots.Length)
        {
            Debug.LogError("Too many players: " + gm.playerCount());
            return;
        }
        for (int i = 0; i < gm.playerCount(); i++)
        {
            NetworkPlayer player = gm.getPlayerByID(i);
            playerSlots[i].setPlayer(player);
            if (player.creatingAvatar){
                playerSlots[i].setState(PlayerSlot.State.CreatingCharacter);
            } else if (player.isConnected)
            {
                playerSlots[i].setState(PlayerSlot.State.Joined);
            }
        }
        if (playerSlots.Length > gm.playerCount())
        {
            for (int i = gm.playerCount(); i < playerSlots.Length; i++)
            {
                playerSlots[i].setPlayer(null);
                playerSlots[i].setState(PlayerSlot.State.Empty);
            }
        }
    }

    public void startGame()
    {
        // First ensure we have enough players
        if (GameManager.Instance.playerCount() < 3)
        {
            for (int i = GameManager.Instance.playerCount(); i < 3; i++)
            {
                GameManager.Instance.addAIPlayer();
            }
        }

        // READY GO
        GameManager.Instance.startGame();
    }
}
