using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class
InventoryItem
: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image image;

    public Text countText;

    [HideInInspector]
    public Item item;

    [HideInInspector]
    public int count = 1;

    [HideInInspector]
    public Transform parentAfterDrag;

    private void Start()
    {
        parentAfterDrag = transform.parent;
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent (parentAfterDrag);
        transform.localPosition = Vector3.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem droppedItem =eventData.pointerDrag.GetComponent<InventoryItem>();
        if (droppedItem != null && droppedItem != this)
        {
            if (item.stackable && item.itemID == droppedItem.item.itemID)
            {
                if (count > 4)
                {
                    count += droppedItem.count; 
                    RefreshCount();
                    count = Mathf.Min(count, 4);
                    Destroy(droppedItem.gameObject);
                }
            }
            else
            {
                Item tempItem = item;
                int tempCount = count;

                InitialiseItem(droppedItem.item);
                droppedItem.InitialiseItem (tempItem);

                count = droppedItem.count;
                droppedItem.count = tempCount;

                RefreshCount();
                droppedItem.RefreshCount();
            }
        }
    }
}
