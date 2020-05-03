using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // PUBLIC FIELDS
    // int ID that we get from ID factory
    public int PlayerID;
    // a Character Asset that contains data about this Hero
    public CharacterAsset charAsset;
    // a script with references to all the visual game objects for this player
    public PlayerArea PArea;
 
    /*// a script of type Spell effect that will be used for our hero power
    // (essenitially, using hero power is like playing a spell in a way)
    public SpellEffect HeroPowerEffect;
    // a flag not to use hero power twice
    public bool usedHeroPowerThisTurn = false;*/

    // REFERENCES TO LOGICAL STUFF THAT BELONGS TO THIS PLAYER
    
    public Deck deck;
    public Hand hand;
    public Table table;

    // a static array that will store both players, should always have 2 players
    //public static Player[] Players;

    // LOL jk we've got one.  This represents the local player.
    public static Player Instance;

    // this value used exclusively for our coin spell
    //private int bonusManaThisTurn = 0;


    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get{ return PlayerID; }
    }

    // opponent player
   /* public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }*/

    // CODE FOR EVENTS TO LET CREATURES KNOW WHEN TO CAUSE EFFECTS
    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;


    
    // ALL METHODS
    void Awake()
    {
        Instance = this;
        // find all scripts of type Player and store them in Players array
        // (we should have only 2 players in the scene)
        //Players = GameObject.FindObjectsOfType<Player>();
        // obtain unique id from IDFactory
        PlayerID = IDFactory.GetUniqueID();
        deck = GameManager.Instance.ItemDeck();
        hand = new Hand();
    }

    public virtual void OnTurnStart()
    {
        Debug.Log("In ONTURNSTART for "+ gameObject.name);
    }

    public void OnTurnEnd()
    {
        if(EndTurnEvent != null)
            EndTurnEvent.Invoke();
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    // FOR TESTING ONLY
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DrawACard();
    }

    // draw a single card from the deck
    public void DrawACard(bool fast = false)
    {
        if (deck.cardCount() > 0)
        {
            if (hand.cardCount() < PArea.handVisual.slots.Children.Length)
            {
                CardLogic newCard = deck.DealCard();
                newCard.owner = this;
                
                hand.addCard(newCard);
                GameManager.Instance.getPlayerByID(GameManager.Instance.localPlayer).addCardToHand(newCard);

                new DrawACardCommand(hand.cardAtIndex(0), this, fast, fromDeck: true).AddToQueue(); 
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }
       
    }

    public void TakeCardBackIntoHand(CardLogic cardLogic, GameObject card)
    {
        if (hand.cardCount() < PArea.handVisual.slots.Children.Length)
        {
            // 1) logic: add card to hand
            cardLogic.owner = this;
            hand.addCard(cardLogic);
            GameManager.Instance.getPlayerByID(GameManager.Instance.localPlayer).addCardToHand(cardLogic);
        }
    }

    // 2 METHODS FOR PLAYING SPELLS
    // 1st overload - takes ids as arguments
    // it is cnvenient to call this method from visual part

    public void PlayItemFromHand(int UniqueID, int tablePos)
    {
        PlayItemFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);
    }

    public void PlayItemFromHand(CardLogic playedCard, int tablePos)
    {
        table.PlaceCardAt(tablePos, playedCard);
        new PlayAnItemCommand(playedCard, this, tablePos, playedCard.UniqueCardID).AddToQueue();
        hand.removeCard(playedCard);
        GameManager.Instance.getPlayerByID(GameManager.Instance.localPlayer).removeCardFromHand(playedCard);
    }

     // START GAME METHODS
    public void LoadCharacterInfoFromAsset()
    {
    //    Health = charAsset.MaxHealth;
        // change the visuals for portrait, hero power, etc...
        PArea.Portrait.charAsset = charAsset;
        PArea.Portrait.ApplyLookFromAsset();
    }

    public void TransmitInfoAboutPlayerToVisual()
    {
        PArea.Portrait.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
        if (GetComponent<TurnMaker>() is AITurnMaker)
        {
            // turn off turn making for this character
            PArea.AllowedToControlThisPlayer = false;
        }
        else
        {
            // allow turn making for this character
            PArea.AllowedToControlThisPlayer = true;
        }
    }
       
        
}
