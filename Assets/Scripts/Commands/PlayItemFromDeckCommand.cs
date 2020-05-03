using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemFromDeckCommand : Command
{
    private Player p;
    private CardLogic cl;
    private bool fast;
    private Slot toSlot;

    public PlayItemFromDeckCommand(CardLogic cl, Player p, bool fast, Slot toSlot)
    {
        this.cl = cl;
        this.p = p;
        this.fast = fast;
        this.toSlot = toSlot;
    }

    public override void StartCommandExecution()
    {
        p.PArea.PDeck.CardsInDeck--;
        toSlot.GetCardFromDeck(cl.ca, cl.UniqueCardID, fast);
    }
}