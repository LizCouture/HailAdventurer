using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI roomNameLabel;
    public PlayerSlot[] playerSlots;

    public NetworkPlayer localPlayer;
    public GameManager gm = GameManager.Instance;

    [SerializeField]
    protected string roomName;

    const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const int roomNameChars = 5;

    public GameObject popup;

    // Start is called before the first frame update
    void Awake()
    {
        gm = GameManager.Instance;
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
        ccm.OpenMenu(gm.localPlayer);
    }

    public void updateSlotPlayers()
    {
        gm = GameManager.Instance;
        Debug.Log("updateSlotPlayers");
        Debug.Log("gm.Players.Count: " + gm.ToString());
        Debug.Log("playerSlots.Length: " + playerSlots.Length);
        if(gm.Players.Count > playerSlots.Length)
        {
            Debug.LogError("Too many players: " + gm.Players.Count);
            return;
        }
        for (int i = 0; i < gm.Players.Count; i++)
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
        if (playerSlots.Length > gm.Players.Count)
        {
            for (int i = gm.Players.Count; i < playerSlots.Length; i++)
            {
                playerSlots[i].setPlayer(null);
                playerSlots[i].setState(PlayerSlot.State.Empty);
            }
        }
    }
}
