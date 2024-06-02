using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public Transform player;  // Assign this via the Inspector to reference the Player GameObject's Transform
    public InventoryManager inventoryMenager;
    public GameObject chestUI;  // Reference to the UI panel GameObject that should be toggled
    public float interactionDistance = 3f;  // Distance within which the player can interact with the chest
    private bool isOpen = false;  // To keep track of the chest's open/close state

    public InventorySlot[] chestSlots;  // Slots in the chest
    private List<InventoryItem> chestItems = new List<InventoryItem>();

    void Start()
    {
        if (chestUI != null)
        {
            chestUI.SetActive(false);  // Ensure the UI is hidden initially
        }
    }

    void Update()
    {
        bool open = Openchest();
        // Use the IsPlayerNearby method to check if the player is within the interaction distance
        if (IsPlayerNearby())
        {
            // Listen for the 'E' key to toggle the chest
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleChest();
                
            }
        }
        else if (open)
        {
            // Automatically close the chest and UI if the player moves away
            CloseChest();            
        }
    }

    public bool IsPlayerNearby()
    {
        return Vector3.Distance(player.position, transform.position) <= interactionDistance;
    }

    private void ToggleChest()
    {
        isOpen = !isOpen;  // Toggle the state
        if (isOpen)
        {
            Debug.Log("Chest opened!");
            // Collect items when the chest is opened
        }
        else
        {
            Debug.Log("Chest closed!");
        }
        UpdateChestUI();
    }

    private void CloseChest()
    {
        isOpen = false;
        UpdateChestUI();
        Debug.Log("Chest closed due to distance!");
    }

    private void UpdateChestUI()
    {
        if (isOpen)
        {
            if (chestUI != null)
            {
                chestUI.SetActive(true);  // Show the UI when the chest is opened
            }
        }
        else
        {
            if (chestUI != null)
            {
                chestUI.SetActive(false);  // Hide the UI when the chest is closed
            }
        }
    }

public Dictionary<string, int> GetItemsFromChestWithCounts()
{
    Dictionary<string, int> itemCounts = new Dictionary<string, int>();  // Dictionary to store item counts

    if (chestSlots == null)
    {
        Debug.LogWarning("chestSlots is null");
        return itemCounts;
    }

    // Check each slot in the chest
    for (int i = 0; i < chestSlots.Length; i++)
    {
        InventorySlot slot = chestSlots[i];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null && itemInSlot.item != null)
        {
            string itemName = itemInSlot.item.itemName;

            // Add item counts to the dictionary
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

    // Display the total count of each item
    foreach (var item in itemCounts)
    {
        Debug.Log($"Item: {item.Key}, Total Quantity: {item.Value}");
    }

    return itemCounts;  // Return the dictionary containing item names and their respective counts
}

    public bool Openchest(){
        if(chestUI.activeSelf){
                        Debug.Log("OTWARTA SKRZYNIA");
                  return true;
        }else{
              Debug.Log("ZAMKNIETA SKRZYNIA");
            return false;
        }
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
