using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{

    public Transform player;  
        public InventoryManager inventoryMenager;
    public InventorySlot[] chestSlots; 
    private List<InventoryItem> chestItems = new List<InventoryItem>();

public Dictionary<string, int> GetItemsFromChestWithCounts()
{
    Dictionary<string, int> itemCounts = new Dictionary<string, int>();  

    if (chestSlots == null)
    {
        Debug.LogWarning("chestSlots is null");
        return itemCounts;
    }

    for (int i = 0; i < chestSlots.Length; i++)
    {
        InventorySlot slot = chestSlots[i];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null && itemInSlot.item != null)
        {
            string itemName = itemInSlot.item.itemName;
            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName] += itemInSlot.count;
            }
            else
            {
                itemCounts[itemName] = itemInSlot.count;
            }

            Debug.Log($"Slot {i}: {itemName}, Quantity: {itemInSlot.count}");
        }
        else
        {
            Debug.Log($"Slot {i}: Empty");
        }
    }
    foreach (var item in itemCounts)
    {
        Debug.Log($"Item: {item.Key}, Total Quantity: {item.Value}");
    }

    return itemCounts;  
}    
    public bool RemoveItemChest(string itemName, int quantity)
    {
        int remainingQuantity = quantity;

        foreach (InventorySlot slot in chestSlots)
        {
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.name == itemName)
            {
                if (itemInSlot.count >= remainingQuantity)
                {
                    itemInSlot.count -= remainingQuantity;
                    if (itemInSlot.count == 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    itemInSlot.RefreshCount();
                    Debug.Log($"Usunieto {quantity} o nazwie '{itemName}'. W miejscu: {itemInSlot.count}");
                    return true;
                }
                else
                {
                    remainingQuantity -= itemInSlot.count;
                    Destroy(itemInSlot.gameObject); 
                }
            }
        }

        Debug.Log($"Nie usunieto pemnej ilosci '{itemName}'. Potrzeba jest: {quantity}, Usunieto: {quantity - remainingQuantity}");
        return false;
    }

    
    public bool AddItemToChest(Item item)
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (
                itemInSlot != null &&
                itemInSlot.item == item 
            )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                inventoryMenager.SpawnNewItem (item, slot);
                return true;
            }
        }
        return false;
    }


}
