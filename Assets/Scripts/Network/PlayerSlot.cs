using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    public GameObject EmptySlot;
    public GameObject PlayerTag;
    public GameObject InProgressSlot;
    public TextMeshProUGUI PlayerName;
    public Image Avatar;
    public CharacterAsset characterAsset;

    public NetworkPlayer player;
    public enum State { Empty, CreatingCharacter, Joined }
    [SerializeField]
    private State slotState;

    // Start is called before the first frame update
    void Start()
    {
        setState(State.Empty);
    }

    public void setPlayer(NetworkPlayer newPlayer)
    {
        player = newPlayer;
    }

    private void fillFromPlayer()
    {
        Debug.Log("FillFromPlayer characterAsset: " + player.avatar);
        if (player.nickname == null)
            PlayerName.text = "Noname";
        else PlayerName.text = player.nickname;
        characterAsset = player.avatar;
        PlayerName.color = characterAsset.PlayerTextTint;
        Debug.Log("Avatar: " + Avatar.ToString());
        Debug.Log("characterAsset.Image: " + characterAsset.AvatarImage.ToString());
        Avatar.sprite = characterAsset.AvatarImage;
    }

    public void playerLeft()
    {
        characterAsset = null;
        player = null;
        setState(State.Empty);
    }

    public void setState(State state)
    {
        Debug.Log("setState: " + state.ToString());
        slotState = state;
        stateChangeHappened();
    }

    private void stateChangeHappened()
    {
        Debug.Log("stateChangeHappened");
        switch (slotState)
        {
            case State.Empty:
                EmptySlot.SetActive(true);
                InProgressSlot.SetActive(false);
                PlayerTag.SetActive(false);
                break;
            case State.CreatingCharacter:
                InProgressSlot.SetActive(true);
                EmptySlot.SetActive(false);
                PlayerTag.SetActive(false);
                break;
            case State.Joined:
                fillFromPlayer();
                EmptySlot.SetActive(false);
                PlayerTag.SetActive(true);
                InProgressSlot.SetActive(false);
                break;
            default:
                break;
        }
    }
}
