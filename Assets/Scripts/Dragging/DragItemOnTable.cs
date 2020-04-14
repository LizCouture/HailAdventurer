using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragItemOnTable : DraggingActions
{
    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;

    private LineRenderer lr;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject Target;

    public override bool CanDrag
    {
        get
        {
            return true;
            //return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    private void Awake()
    {
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "Above Everything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        whereIsCard = GetComponentInParent<WhereIsTheCardOrCreature>();
        manager = GetComponentInParent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        lr.enabled = true;
        //whereIsCard.BringToFront();
    }

    public override void OnDraggingInUpdate()
    {
        //Draw arrow
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if(notNormalized.magnitude > distanceToTarget)
        {
            //draw line between card and target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            //position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1f * direction;

            //rotate arrow
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        } else
        {
            // if target not far away enough do not show arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }
     }

    public override void OnEndDrag()
    {
        if (DragSuccessful())
        {
            Debug.Log("Drag successful");
            // Determine to place card in left or right slot, based on if the user dragged to slot or table.
            if (DragTargetedLeftSlot())
            {

                Debug.Log("Drag targeted Left spot, playing to 0.");
                playerOwner.PlayItemFromHand(GetComponentInParent<IDHolder>().UniqueID, 0);
            }
            else if (DragTargetedRightSlot())
            {
                Debug.Log("Drag targeted Right slot, playing to 1");
                playerOwner.PlayItemFromHand(GetComponentInParent<IDHolder>().UniqueID, 1);
            }
            else
            {
                Debug.Log("Dragged to table, playing wherever.");
                int tablePos = playerOwner.PArea.tableVisual.TablePosForNewCard(Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z -
                    Camera.main.transform.position.z)).x);
                playerOwner.PlayItemFromHand(GetComponentInParent<IDHolder>().UniqueID, tablePos);
            }
        } else
        {
            // Set old sorting order
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = tempState;
            //Move this card back to its slot position
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        }
        transform.localPosition = new Vector3(0f, 0f, 0.4f);
        lr.enabled = false;
        triangleSR.enabled = false;
    }

    protected override bool DragSuccessful()
    {

        bool TableNotFull = (Table.Instance.SlotLeft == null || Table.Instance.SlotRight == null);
       if (DragTargetedLeftSlot() && Table.Instance.SlotLeft == null)
        {
            return true;
        } else if (DragTargetedRightSlot() && Table.Instance.SlotRight == null)
        {
            return true;
        } else if (DragTargetedTable() && TableNotFull)
            return true;
        else
            return false;
    }

    protected bool DragTargetedTable()
    {
        return TableVisual.Instance.CursorOverTable;
    }

    protected bool DragTargetedLeftSlot()
    {
        return TableVisual.Instance.CursorOverLeftSlot;
    }

    protected bool DragTargetedRightSlot()
    {
        return TableVisual.Instance.CursorOverRightSlot;
    }
}