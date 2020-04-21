using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardLogic: IIdentifiable
{
    // reference to a player who holds this card in his hand
    public Player owner;
    // an ID of this card
    public int UniqueCardID; 
    // a reference to the card asset that stores all the info about this card
    public CardAsset ca;
    // a script of type spell effect that will be attached to this card when it`s created
    //public SpellEffect effect;


    // STATIC (for managing IDs)
    public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();


    // PROPERTIES
    public int ID
    {
        get{ return UniqueCardID; }
    }

   // public int CurrentManaCost{ get; set; }

    public bool CanBePlayed
    {
        get
        {
           // TODO: Set this from EventTYpe
            return true;
        }
    }

    // CONSTRUCTOR
    public CardLogic(CardAsset ca)
    {
        // set the CardAsset reference
        this.ca = ca;
        // get unique int ID
        UniqueCardID = IDFactory.GetUniqueID();
        //UniqueCardID = IDFactory.GetUniqueID();
        //ResetManaCost();
        // create an instance of SpellEffect with a name from our CardAsset
        // and attach it to 
        /*if (ca.SpellScriptName!= null && ca.SpellScriptName!= "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
        }*/
        // add this card to a dictionary with its ID as a key
        CardsCreatedThisGame.Add(UniqueCardID, this);
    }

/*    // method to set or reset mana cost
    public void ResetManaCost()
    {
        CurrentManaCost = ca.ManaCost;
    }*/
    void Destroy()
    {
        Debug.LogError("CARD LOGIC GOT DESTROYED.  SHOULD THIS HAPPEN???");
    }
}
