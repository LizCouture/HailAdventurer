using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class CarouselSelector : MonoBehaviour
{
    public List<Transform> InitialItems;
    public List <Transform> Items;
    public Button leftButton;
    public Button rightButton;

    [SerializeField]
    private int selectedIndex;

    public Transform leftmostItemStart;
    public Transform rightmostItemStart;
    private Vector3 centerpointStart;

    private const float ZOOM_LEVEL = 3.5f;
    private const float MOVE_TIME = 0.2f;


    // Start is called before the first frame update
    void Awake()
    {
        //TODO: Should I put this back in there?
        //initializeItems(InitialItems);
    }

    public void initializeItems(List<Transform> desiredItems)
    {
        Items = desiredItems;
        setupCarousel();
    }

    public void setupCarousel()
    {
        centerpointStart = findMidpoint(leftmostItemStart.position, rightmostItemStart.position);
        centerpointStart = new Vector3(centerpointStart.x, centerpointStart.y, -ZOOM_LEVEL);

        Debug.Log("Left: " + leftmostItemStart + " Center : " + centerpointStart + " Right: " + rightmostItemStart);
        distributeItemsEvenly();
        selectItem(0);
    }

    public void cleanupCarousel()
    {
        while (Items.Count > 0)
        {
            Destroy(Items[0].gameObject);
            Items.RemoveAt(0);
        }
    }

    private void selectItem(int index)
    {
        selectedIndex = index;
        centerCarouselOnIndex(index);
    }

    private void centerCarouselOnIndex(int index)
    {
        Debug.Log("Centering carousel on index: " + index);
        Sequence s = DOTween.Sequence();

        // First, move selected item to the center.
        s.Append(Items[index].DOMove(centerpointStart, MOVE_TIME));

        // If there is only one item to the left or right, move that item to the left or right position.
        // If there are more than one item to the left or right, distribute the items evenly.
        if (index > 0)
        {
            if (index == 1)
            {
                s.Append(Items[0].DOMove(leftmostItemStart.position, MOVE_TIME));
            }
            else
            {
                distributeEvenlyBetweenPoints(leftmostItemStart.position, centerpointStart, 0, index);
            }
        }
        if(index < Items.Count - 1)
        {
            if (index == Items.Count - 2)
            {
                s.Append(Items[Items.Count - 1].DOMove(rightmostItemStart.position, MOVE_TIME));
            }
            else
            {
                distributeEvenlyBetweenPoints(centerpointStart, rightmostItemStart.position, index, Items.Count - 1);
            }
        }
        Debug.Log("Left: " + leftmostItemStart + " Center : " + centerpointStart + " Right: " + rightmostItemStart);
    }
    
    void distributeEvenlyBetweenPoints(Vector3 left, Vector3 right, int firstItem, int lastItem)
    {
        if (firstItem == lastItem)
        {
            Debug.LogError("Tried to distribute one item: " + firstItem);
        }
        else
        {

            Sequence s = DOTween.Sequence();
            float XDist = (right.x - left.x) / (float)(lastItem - firstItem);
            float YDist = (right.y - left.y) / (float)(lastItem - firstItem);
            float ZDist = (right.z - left.z) / (float)(lastItem - firstItem);

            Vector3 Dist = new Vector3(XDist, YDist, ZDist);

            //s.Append(Items[firstItem].DOMove(left, MOVE_TIME));
            for (int i = 0; i < lastItem - firstItem; i++)
            {
                s.Append(Items[firstItem + i].DOMove(left + (Dist * i), MOVE_TIME));
            }
        }
    }

    // Use this for initialization
    void distributeItemsEvenly()
    {
        distributeEvenlyBetweenPoints(Items[0].position, Items[Items.Count - 1].position, 0, Items.Count - 1);
    }

    Vector3 findMidpoint(Vector3 left, Vector3 right)
    {
        return new Vector3((left.x + (right.x-left.x)/2), (left.y + (right.y-left.y)/2), (left.z + (right.z-left.z)/2));
    }

    public void OnClickLeft()
    {
        if (selectedIndex > 0)
        {
            selectItem(selectedIndex - 1);
        }
    }

    public void OnClickRight()
    {
        if (selectedIndex < (Items.Count - 1))
        {
            selectItem(selectedIndex + 1);
        }
    }

    public int GetSelection()
    {
        return selectedIndex;
    }

}
