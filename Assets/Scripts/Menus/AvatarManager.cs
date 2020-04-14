using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    public List<CharacterAsset> AvatarDictionary = new List<CharacterAsset>();

    // Start is called before the first frame update
    void Start()
    {
        AvatarDictionary.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
