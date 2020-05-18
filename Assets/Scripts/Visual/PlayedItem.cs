using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PlayedItem : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;

    [SerializeField]
    private CardAsset card1Asset;
    [SerializeField]
    private CardAsset card2Asset;


    public string nickname;
    public TextMeshProUGUI nicknameField;
    public CharacterAsset avatarAsset;
    public Image avatarSprite;

    public NetworkPlayer np;

    public void LoadFromNetworkPlayer(NetworkPlayer np)
    {
        this.np = np;
        nickname = np.nickname;
        nicknameField.text = nickname;

        avatarAsset = np.avatar;
        avatarSprite.sprite = avatarAsset.AvatarImage;

        if (!np.cardsPlayed.Any() || np.cardsPlayed.Count < 2)
        {
            Debug.LogError("ERROR:  Loading PlayedItem.cs from Player who didn't play any cards.");
        } else
        {
            card1Asset = np.cardsPlayed[0].ca;
            card2Asset = np.cardsPlayed[1].ca;

            OneCardManager card1Manager = card1.GetComponent<OneCardManager>();
            card1Manager.cardAsset = card1Asset;
            card1Manager.ReadCardFromAsset();
            OneCardManager card2Manager = card2.GetComponent<OneCardManager>();
            card2Manager.cardAsset = card2Asset;
            card2Manager.ReadCardFromAsset();
        }
    }
}
