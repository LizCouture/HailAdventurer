using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSelector : MonoBehaviour
{
    public List<GameObject> avatarSlots;
    public List<CharacterAsset> avatarAssets;

    public CharacterAsset selectedAvatar;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;

    }

    public void initializeAvatars()
    {
        avatarAssets = new List<CharacterAsset>();
        int numSlots = avatarSlots.Count;
        for (int i = 0; i < numSlots; i++)
        {
            CharacterAsset newAv = gm.avatarManager.DealAvatar();
            avatarAssets.Add(newAv);
        }

        fillSlots();
    }
    private void fillSlots()
    {
        for(int i = 0; i < avatarSlots.Count; i++)
        {
            styleSlotByAsset(avatarSlots[i], avatarAssets[i]);
        }
    }

    private void styleSlotByAsset(GameObject slot, CharacterAsset asset)
    {
        GameObject imageObject = slot.transform.Find("Image").gameObject;
        imageObject.GetComponent<Image>().sprite = asset.AvatarImage;
    }

    public void SelectAvatar(int slot)
    {
        selectedAvatar = avatarAssets[slot];
        gameObject.transform.parent.transform.parent.GetComponent<CreateCharacterMenu>().reflectChoice();
    }
}
