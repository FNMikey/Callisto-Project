using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image; // Przypisz komponent Image

    public Color selectedColor = Color.yellow;
    public Color notSelectedColor = Color.white;

    public string selectedItemName = "";

    private void Awake()
    {
        Deselect();
    }

    public void Select(){
        image.color = selectedColor;
        InventoryItem inventoryItem = GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null)
        {
            selectedItemName = inventoryItem.item.name;
            Debug.Log(selectedItemName);
        }
    }


        public void Deselect(){
        image.color = notSelectedColor;
        selectedItemName = "";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
  
    public string GetSelectedItemName()
    {
        return selectedItemName;
    }
}
