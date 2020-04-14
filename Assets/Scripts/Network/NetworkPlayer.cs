using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer 
{
    public CharacterAsset avatar;
    public string nickname;
    public List<CardLogic> hand;
    public List<CardLogic> cardsPlayed;
    public int coins;

    public bool isConnected;
    public bool creatingAvatar;
    public bool isAI;

    public NetworkPlayer()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
