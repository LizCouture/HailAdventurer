using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    [SerializeField]
    protected List<CharacterAsset> AvatarDictionary = new List<CharacterAsset>();

    // Start is called before the first frame update
    void Start()
    {
        AvatarDictionary.Shuffle();
    }

    public CharacterAsset DealAvatar()
    {
        Debug.Log("Dealing avatar: " + AvatarDictionary[0].name);
        CharacterAsset newAvatar = AvatarDictionary[0];
        AvatarDictionary.RemoveAt(0);
        return newAvatar;
    }

    public void ReturnToDeck(CharacterAsset asset)
    {
        AvatarDictionary.Add(asset);
    }

    public void ShuffleDictionary()
    {
        Debug.Log("Shuffling Dictionary");
        AvatarDictionary.Shuffle();
    }

    public int DictLength()
    {
        return AvatarDictionary.Count;
    }
}
