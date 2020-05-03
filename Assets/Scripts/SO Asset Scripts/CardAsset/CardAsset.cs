using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardAsset : ScriptableObject 
{
    // this object will hold the info about the most general card
    [Header("General info")]

    [TextArea(2,3)]
    public string Description;  // Description for spell or character
	public Sprite CardImage;

    [Header("Item Info")]
    public string ItemScriptName;

    [Header("Adventurer Info")]
    public string AdventurerScriptName;

    public override string ToString()
    {
        return CardImage.name;
    }



}
