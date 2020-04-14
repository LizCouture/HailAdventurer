using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TableVisual : MonoBehaviour 
{
    // PUBLIC FIELDS

    // an enum that mark to whish caracter this table belongs. The alues are - Top or Low
    public AreaPosition owner;
    public Player ownerPlayer;

    // a referense to a game object that marks positions where we should put new Creatures
    public SameDistanceChildren slots;
    public GameObject leftSlot;
    public GameObject rightSlot;

    // PRIVATE FIELDS

   // list of the cards in those slots
   [SerializeField]
    private GameObject leftCardOnTable = null;
    [SerializeField]
    private GameObject rightCardOnTable = null;

    // are we hovering over this table`s collider with a mouse
    private bool cursorOverThisTable = false;
    private bool cursorOverLeftSlot = false;
    private bool cursorOverRightSlot = false;

    // A 3D collider attached to this game object
    private BoxCollider col;
    private BoxCollider leftSlotCol;
    private BoxCollider rightSlotCol;

    public static TableVisual Instance;

    // PROPERTIES
    // returns true only if we are hovering over this table`s collider
    public bool CursorOverTable
    {
        get{ return cursorOverThisTable; }
    }

    public bool CursorOverLeftSlot
    {
        get { return cursorOverLeftSlot; }
    }

    public bool CursorOverRightSlot
    {
        get { return cursorOverRightSlot; }
    }

    // METHODS

    // MONOBEHAVIOUR METHODS (mouse over collider detection)
    void Awake()
    {
        Instance = this;
        col = GetComponent<BoxCollider>();
        rightSlotCol = rightSlot.GetComponent<BoxCollider>();
        leftSlotCol = leftSlot.GetComponent<BoxCollider>();
        if (owner == AreaPosition.Top) { ownerPlayer = Player.Players[0]; }
        else if (Player.Players.Length > 1)
        {
            ownerPlayer = Player.Players[1];
        }
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        // we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
        // create an array of RaycastHits
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        cursorOverLeftSlot = false;
        cursorOverRightSlot = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == col)
                passedThroughTableCollider = true;
            if (h.collider == rightSlotCol)
                cursorOverRightSlot = true;
            if (h.collider == leftSlotCol)
                cursorOverLeftSlot = true;
                
        }
        cursorOverThisTable = passedThroughTableCollider;
    }
   
    // method to create a new creature and add it to the table
    public void AddCardAtIndex(CardAsset ca, GameObject itemCard, int UniqueID ,int index)
    {
        GameObject targetSlot;
        if (index == 0) targetSlot = leftSlot;
        else targetSlot = rightSlot;

        Debug.Log("Adding CardAtIndex: " + ca.ToString() + itemCard.ToString() + UniqueID.ToString() + index);
        // create a new creature from prefab
       // GameObject itemCard = GameObject.Instantiate(GlobalSettings.Instance.ItemCardPrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;
       itemCard.transform.position = slots.Children[index].transform.position;

        // apply the look from CardAsset
        OneCardManager manager = itemCard.GetComponent<OneCardManager>();
        //manager.cardAsset = ca;
        //manager.ReadCardFromAsset();

        // add tag according to owner
        foreach (Transform t in itemCard.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString()+"Item";
        
        // parent a new creature gameObject to table slots
        itemCard.transform.SetParent(targetSlot.transform);

        // add a new creature to the list
        if (index == 0)
        {
            leftCardOnTable = itemCard;
        }
        else rightCardOnTable = itemCard;
        
        // let this creature know about its position
        WhereIsTheCardOrCreature w = itemCard.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowTable;
        else
            w.VisualState = VisualStates.TopTable;

        // add our unique ID to this creature
        if (itemCard.GetComponent<IDHolder>() == null)
        {
            Debug.Log("Tried to play a card without an IDHolder in TableVisual.");
            IDHolder id = itemCard.AddComponent<IDHolder>();
            id.UniqueID = UniqueID;
        }

        // after a new creature is added update placing of all the other creatures
        //ShiftSlotsGameObjectAccordingToNumberOfCards();
        //PlaceCardsOnNewSlots();

        // end command execution
        Command.CommandExecutionComplete();
    }


    // returns an index for a new creature based on mousePosition
    // included for placing a new creature to any positon on the table
    public int TablePosForNewCard(float MouseX)
    {
        // If one of the slots is empty, and card is dragged to table, return empty slot.
        if (Table.Instance.SlotLeft == null && Table.Instance.SlotRight != null)
        {
            Debug.Log("Left Slot empty");
            return 0;
        }
        if (Table.Instance.SlotRight == null && Table.Instance.SlotLeft != null)
        {
            Debug.Log("Right Slot empty");
            return 1;
        }

        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.
        if (MouseX < slots.Children[0].transform.position.x)
        {
            Debug.Log("Both Empty, Mouse Left");
            return 0;
        }
        else
        {
            Debug.Log("Other");
            return 1;
        }
    }

    // Destroy a creature
    public void RemoveCardWithID(int IDToRemove)
    {
        // TODO: This has to last for some time
        // Adding delay here did not work because it shows one creature die, then another creature die. 
        // 
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(1f);
        //s.OnComplete(() =>
        //   {
                
        //    });
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        //CardsOnTable.Remove(creatureToRemove);
        if (leftCardOnTable == creatureToRemove) leftCardOnTable = null;
        if (rightCardOnTable == creatureToRemove) rightCardOnTable = null;
        Destroy(creatureToRemove);

        //ShiftSlotsGameObjectAccordingToNumberOfCards();
        //PlaceCardsOnNewSlots();
        Command.CommandExecutionComplete();
    }

    public void SendCardBackToHand(int slot)
    {
        if ((slot == 0 && leftCardOnTable != null )|| (slot == 1 && rightCardOnTable != null)) {
            GameObject returnCard;
            if (slot == 0)
            {
                returnCard = leftCardOnTable;
                leftCardOnTable = null;
            }
            else
            {
                returnCard = rightCardOnTable;
                rightCardOnTable = null;
            }
            Table.Instance.ReturnCardToHandFrom(slot, returnCard, ownerPlayer);
            }
        else
        {
            Debug.LogError("Trying to send card back to hand, but nothing in slot.");
        }
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
   /* void ShiftSlotsGameObjectAccordingToNumberOfCards()
    {
        float posX;
        if (CardsOnTable.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CardsOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }*/

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
   /* void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsOnTable)
        {
            g.transform.DOLocalMoveX(slots.Children[CardsOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            // TODO: figure out if I need to do something here:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = CreaturesOnTable.IndexOf(g);
        }
    }*/

}
