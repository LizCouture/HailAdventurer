using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine.Progress;

public class CarouselSelector : MonoBehaviour
{
    public Transform[] Items;
    public Button leftButton;
    public Button rightButton;

    [SerializeField]
    private int selectedIndex;

    private Vector3 leftmostItemStart;
    private Vector3 rightmostItemStart;
    private Vector3 centerpointStart;

    private const float ZOOM_LEVEL = 3.5f;

    // Start is called before the first frame update
    void Awake()
    {
        leftmostItemStart = Items[0].position;
        rightmostItemStart = Items[Items.Length - 1].position;
        centerpointStart = findMidpoint(leftmostItemStart, rightmostItemStart);
        centerpointStart = new Vector3(centerpointStart.x, centerpointStart.y, -ZOOM_LEVEL);

        Debug.Log("Left: " + leftmostItemStart + " Center : " + centerpointStart + " Right: " + rightmostItemStart);
        distributeItemsEvenly();
        selectItem(3);

    }

    private void selectItem(int index)
    {
        selectedIndex = index;
        centerCarouselOnIndex(index);
    }

    private void centerCarouselOnIndex(int index)
    {
        Items[index].position = centerpointStart;
        if(index > 0)
        {
            distributeEvenlyBetweenPoints(leftmostItemStart, centerpointStart, 0, index);
        }
        if(index < Items.Length - 1)
        {
            distributeEvenlyBetweenPoints(centerpointStart, rightmostItemStart, index, Items.Length - 1);
        }
        Debug.Log("Left: " + leftmostItemStart + " Center : " + centerpointStart + " Right: " + rightmostItemStart);
    }
    
    void distributeEvenlyBetweenPoints(Vector3 left, Vector3 right, int firstItem, int lastItem)
    {
        float XDist = (right.x - left.x) / (float)(lastItem - firstItem);
        float YDist = (right.y - left.y) / (float)(lastItem - firstItem);
        float ZDist = (right.z - left.z) / (float)(lastItem - firstItem);

        Vector3 Dist = new Vector3(XDist, YDist, ZDist);

        Items[firstItem].position = left;
        for (int i = firstItem+1; i <= lastItem; i++)
        {
            Items[i].position = Items[i - 1].position + Dist;
        }
    }

    // Use this for initialization
    void distributeItemsEvenly()
    {
        distributeEvenlyBetweenPoints(Items[0].position, Items[Items.Length - 1].position, 0, Items.Length - 1);
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
        if (selectedIndex < (Items.Length - 1))
        {
            selectItem(selectedIndex + 1);
        }
    }

}
