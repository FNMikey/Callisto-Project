using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FurnaceManager : MonoBehaviour
{
    public InventorySlot fuelSlot;
    public InventorySlot oreSlot;
    public InventorySlot productSlot;
    public Item item;
    public InventoryManager inventoryManager;
    public float smeltingTime = 10.0f;

    private int time;
    private int productAmount;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // Example trigger to start smelting
        {
            StartSmelting();
        }
    }

    void StartSmelting()
    {
        string inventoryContents;
        if (CheckOreAndFuel(out inventoryContents))
        {
            StartCoroutine(Smelt());
        }
    }

    bool CheckOreAndFuel(out string inventoryContents)
    {
        inventoryContents = "";

        // Debugging: Check if the slots themselves are not null
        if (fuelSlot == null)
        {
            Debug.Log("Fuel slot is null");
            return false;
        }
        if (oreSlot == null)
        {
            Debug.Log("Ore slot is null");
            return false;
        }

        InventoryItem fuelSlotItem = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem oreSlotItem = oreSlot.GetComponentInChildren<InventoryItem>();

        // Debugging: Check if the items in the slots are not null
        if (fuelSlotItem == null)
        {
            Debug.Log("Fuel slot item is missing");
        }
        if (oreSlotItem == null)
        {
            Debug.Log("Ore slot item is missing");
        }

        if (oreSlotItem != null && fuelSlotItem != null)
        {
            Debug.Log("Ore slot item name: " + oreSlotItem.item.itemName);
            Debug.Log("Fuel slot item name: " + fuelSlotItem.item.itemName);

            if (oreSlotItem.item.itemName == "IronOre" && fuelSlotItem.item.itemName == "Stick")
            {
                if (fuelSlotItem.count >= 3)
                {
                    productAmount = (int)Mathf.Ceil(oreSlotItem.count * 0.5f);

                    if (oreSlotItem.count >= 3 && oreSlotItem.count < 7)
                    {
                        time = 5;
                    }
                    else if (oreSlotItem.count >= 70 && oreSlotItem.count < 90)
                    {
                        time = 10;
                    }
                    else if (oreSlotItem.count >= 90 && oreSlotItem.count < 100)
                    {
                        time = 15;
                    }
                    else if (oreSlotItem.count == 100)
                    {
                        time = 20;
                    }

                    return true;
                }
                else
                {
                    Debug.Log("Not enough fuel");
                    return false;
                }
            }
            else
            {
                Debug.Log("Invalid items for smelting");
                return false;
            }
        }
        else
        {
            Debug.Log("Missing items");
            return false;
        }
    }

    IEnumerator Smelt()
    {
        InventoryItem fuelSlotItem = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem oreSlotItem = oreSlot.GetComponentInChildren<InventoryItem>();
        if (fuelSlotItem == null || oreSlotItem == null)
        {
            Debug.Log("Fuel or Ore slot item is null");
            yield break;
        }

        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(time / 5);

            // Decrease fuel count
            if (fuelSlotItem != null)
            {
                fuelSlotItem.count--;
                if (fuelSlotItem.count <= 0)
                {
                    Destroy(fuelSlotItem.gameObject);
                    fuelSlotItem = null; // Prevent further access to this now-null object
                }
                else
                {
                    fuelSlotItem.RefreshCount();
                }
            }

            // Decrease ore count
            if (oreSlotItem != null)
            {
                oreSlotItem.count--;
                if (oreSlotItem.count <= 0)
                {
                    Destroy(oreSlotItem.gameObject);
                    oreSlotItem = null; // Prevent further access to this now-null object
                }
                else
                {
                    oreSlotItem.RefreshCount();
                }
            }
                  InventoryItem productItem = productSlot.GetComponentInChildren<InventoryItem>();
                  Debug.Log("Product item" + productItem);
            if (productItem == null)
            {
                     inventoryManager.SpawnNewItem(item, productSlot);
                    Debug.Log("Ore count after decrement: " + productItem);
                }
                else
                {
                    productItem.count+=productAmount;
                    productItem.RefreshCount();
                }
            
        }
    }

    // New method to be linked with the "Create" button
    public void CreateNewObject()
    {
        StartSmelting();
    }
}