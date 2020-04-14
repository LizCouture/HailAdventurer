using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnItemCommand : Command
{
    private CardLogic cl;
    private int tablePos;
    private Player p;
    private int cardID;

    public PlayAnItemCommand(CardLogic cl, Player p, int tablePos, int cardID)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.cardID = cardID;
    }

    public override void StartCommandExecution()
    {
        //remove and destroy the card in hand
        HandVisual PlayerHand = p.PArea.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
        PlayerHand.RemoveCard(card);
        //GameObject.Destroy(card);
        // enable Hover Previews
        HoverPreview.PreviewsAllowed = true;
        p.PArea.tableVisual.AddCardAtIndex(cl.ca, card, cardID, tablePos);
        // In tableVisual script it calls "Command.ExecutionComplete"
    }
}
