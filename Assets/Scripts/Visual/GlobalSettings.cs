using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GlobalSettings: MonoBehaviour 
{
    [Header("Players")]
    public Player TopPlayer;
    public Player LowPlayer;
    [Header("Colors")]
    public Color32 CardBodyStandardColor;
    //public Color32 CardRibbonsStandardColor;
    //public Color32 CardGlowColor;
    [Header("Numbers and Values")]
    public float CardPreviewTime = 0.05f;
    public float CardTransitionTime= 0.01f;
    public float CardPreviewTimeFast = 0.025f;
    public float CardTransitionTimeFast = 0.005f;
    [Header("Prefabs and Assets")]
    public GameObject ItemCardPrefab;
    public GameObject AdventurerCardPrefab;
//    public GameObject TargetedSpellCardPrefab;
 //   public GameObject CreatureCardPrefab;
 //   public GameObject CreaturePrefab;
 //   public GameObject DamageEffectPrefab;
 //   public GameObject ExplosionPrefab;
    [Header("Other")]
    public Button EndTurnButton;
    //public CardAsset CoinCard;
    public GameObject GameOverPanel;
    //public Sprite HeroPowerCrossMark;

    public Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();


    // SINGLETON
    public static GlobalSettings Instance;

    void Awake()
    {
        Players.Add(AreaPosition.Top, TopPlayer);
        Players.Add(AreaPosition.Low, LowPlayer);
        Instance = this;
    }

    public bool CanControlThisPlayer(AreaPosition owner)
    {
        // TODO: This might need to change to reflect Event, or be removed
        // bool PlayersTurn = (TurnManager.Instance.whoseTurn == Players[owner]);
        bool PlayersTurn = true;
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return Players[owner].PArea.AllowedToControlThisPlayer && Players[owner].PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }

    public bool CanControlThisPlayer(Player ownerPlayer)
    {
        // TODO: This might need to change to reflect Event, or be removed
        //bool PlayersTurn = (TurnManager.Instance.whoseTurn == ownerPlayer);
        bool PlayersTurn = true;
        bool NotDrawingAnyCards = !Command.CardDrawPending();
        return ownerPlayer.PArea.AllowedToControlThisPlayer && ownerPlayer.PArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }

    public void EnableEndTurnButtonOnStart(Player P)
    {
        if (P == LowPlayer && CanControlThisPlayer(AreaPosition.Low) ||
            P == TopPlayer && CanControlThisPlayer(AreaPosition.Top))
            EndTurnButton.interactable = true;
        else
            EndTurnButton.interactable = false;
            
    }
}
