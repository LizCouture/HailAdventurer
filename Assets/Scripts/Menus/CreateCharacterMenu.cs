using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateCharacterMenu : MonoBehaviour
{
    public AvatarSelector avatarSelector;
    public TextMeshProUGUI nicknameText;
    public TMP_InputField nicknameEntry;
    public GameObject overlay;
    public GameObject container;

    public NetworkPlayer player;
    private int playerID;

    private void Start()
    {
        container.SetActive(false);
        overlay.SetActive(false);
    }

    public void OpenMenu(int id)
    {
        player = GameManager.Instance.getPlayerByID(id);
        playerID = id;
        avatarSelector.initializeAvatars();
        overlay.SetActive(true);
        container.SetActive(true);
    }

    public void CancelMenu()
    {
        player = null;
        container.SetActive(false);
        overlay.SetActive(false);
    }

    public void onSubmit()
    {
        player.nickname = nicknameEntry.text;
        player.avatar = avatarSelector.selectedAvatar;

        // Return any unused assets to the deck.
        GameManager gm = GameManager.Instance;
        gm.connectPlayer(playerID);
        AvatarManager am = GameManager.Instance.avatarManager;
        for (int i = 0; i < avatarSelector.avatarAssets.Count; i++)
        {
            if (avatarSelector.avatarAssets[i] != avatarSelector.selectedAvatar)
            {
                am.ReturnToDeck(avatarSelector.avatarAssets[i]);
            }
        }
        am.ShuffleDictionary();

        container.SetActive(false);
        overlay.SetActive(false);
    }

    public void reflectChoice()
    {
        nicknameText.color = avatarSelector.selectedAvatar.PlayerTextTint;
    }

}
