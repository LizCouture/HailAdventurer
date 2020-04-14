using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TakeCardBackCommand : Command
{

    private Player p;
    private GameObject card;
    private CardLogic cl;

    public TakeCardBackCommand(CardLogic cl, GameObject card, Player p)
    {
        this.cl = cl;
        this.p = p;
        this.card = card;
        Debug.Log("Creating TakeCardBackCommand with cl: " + cl.ToString() + " p: " + p.ToString() + " card: " + card.ToString());
    }

    public override void StartCommandExecution()
    {
        // This assumes the Table class has already handled removing this card from the table.
        p.TakeCardBackIntoHand(cl, card);
        p.PArea.handVisual.TakeCardBackIntoHand(card);
        // handVisual will call "Command.ExecutionComplete"
    }
}
