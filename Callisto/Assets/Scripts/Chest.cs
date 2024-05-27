using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public Transform player;  // Assign this via the Inspector to reference the Player GameObject's Transform
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

     public List<InventoryItem> GetItemsFromChest()
    {
        List<InventoryItem> items = new List<InventoryItem>();  // Local list to store items

        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                items.Add(itemInSlot);
                Debug.Log("Added " + itemInSlot.item.name + " to items from slot " + i);
            }
        }

        return items;  // Return the filled list of items
    }

    public bool Openchest(){
        if(isOpen && chestUI.activeSelf){
            return true;
             Debug.Log("OTWARTA SKRZYNIA");
        }else{
              Debug.Log("ZAMKNIETA SKRZYNIA");
            return false;
        }
    }
}
