using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slot : MonoBehaviour
{
    public TableVisual table;
    public int slotNum;

    public AreaPosition owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        table.SendCardBackToHand(slotNum);
    }

    public void GetCardFromDeck(CardAsset ca, int cardID, bool fast)
    {
        GameObject card;
        Transform DeckTransform = table.deckPosition;
        card = CreateACardAtPosition(ca, DeckTransform.position, new Vector3(0f, -179f, 0f));

        // Set a tag to reflect where this card is
        foreach (Transform t in card.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString() + "Card";
        // pass this card to HandVisual class
        AddCard(card);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();
        w.Slot = 0;
        w.VisualState = VisualStates.Transition;

        // pass a unique ID to this card.
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = cardID;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            // Debug.Log ("Not fast!!!");
            s.Append(card.transform.DOLocalMove(gameObject.transform.localPosition, GlobalSettings.Instance.CardTransitionTime));
            s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(gameObject.transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
            s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast));
        }

        s.OnComplete(() => ChangeLastCardStatusToInSlot(card, w));

    }

    GameObject CreateACardAtPosition(CardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject card;
        card = GameObject.Instantiate(GlobalSettings.Instance.ItemCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;

        // apply the look of the card based on the info from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = c;
        manager.ReadCardFromAsset();

        return card;
    }

    public void AddCard(GameObject card)
    {
        // parent this card to our Slots GameObject
        card.transform.SetParent(gameObject.transform);
    }

    private void ChangeLastCardStatusToInSlot(GameObject card, WhereIsTheCardOrCreature w)
    {
        // TODO: Make this a slot-related status.
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowHand;
        else
            w.VisualState = VisualStates.TopHand;

        // end command execution for DrawACArdCommand or TakeCardBackCommand
        Command.CommandExecutionComplete();
    }
}
