using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 4;

    public InventorySlot[] inventorySlots;

    private Chest chest;

    public GameObject inventoryItemPrefab;

    public bool RemoveItem(string itemName, int quantity)
    {
        int remainingQuantity = quantity;

        foreach (InventorySlot slot in inventorySlots)
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

    public bool IsInventoryFull()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (
                itemInSlot == null ||
                itemInSlot.count < maxStackedItems && itemInSlot.item.stackable
            )
            {
                return false;
            }
        }
        return true;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (
                itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable
            )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem (item, slot);
                return true;
            }
        }
        return false;
    }
    
    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem (item);
    }

   public bool CheckRecipeIngredients(Recipe recipe, out string inventoryContents)
{
    // Check for null input
    if (recipe == null || recipe.ingredients == null)
    {
        inventoryContents = "Error: Recipe or ingredients list is null.";
        return false;  // Cannot proceed without a valid recipe
    }

    Dictionary<string, int> inventoryCounts = new Dictionary<string, int>();
    inventoryContents = "";
    bool canCraft = true;

    // Initialize inventory counts with recipe ingredients
    foreach (Ingredient ingredient in recipe.ingredients)
    {
        if (ingredient != null)  // Check for null ingredients
        {
            inventoryCounts[ingredient.name] = 0;
        }
    }

    // Add counts from the inventory
    for (int i = 0; i < inventorySlots.Length; i++)
    {
        InventorySlot slot = inventorySlots[i];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null && itemInSlot.item != null)
        {
            if (inventoryCounts.ContainsKey(itemInSlot.item.itemName))
            {
                inventoryCounts[itemInSlot.item.itemName] += itemInSlot.count;
            }

            inventoryContents += $"Slot {i}: {itemInSlot.item.itemName}, Quantity: {itemInSlot.count}\n";
        }
        else
        {
            inventoryContents += $"Slot {i}: Empty\n";
        }
    }

    // Check the chest if it's open and get items
    if (chest.Openchest())
    {
        List<InventoryItem> itemsFromChest = chest.GetItemsFromChest();
        foreach (InventoryItem item in itemsFromChest)
        {
            if (item != null && item.item != null && inventoryCounts.ContainsKey(item.item.itemName))
            {
                inventoryCounts[item.item.itemName] += item.count;
            }
        }
    }

    // Summary of required items from the recipe
    inventoryContents += "\nSummary of required items from the recipe:\n";
    foreach (Ingredient ingredient in recipe.ingredients)
    {
        if (ingredient != null)
        {
            int countNeeded = ingredient.quantity;
            int countAvailable = inventoryCounts.ContainsKey(ingredient.name) ? inventoryCounts[ingredient.name] : 0;
            inventoryContents += $"{ingredient.name}: Needed {countNeeded}, Available {countAvailable}\n";
            if (countAvailable < countNeeded)
            {
                canCraft = false;
            }
        }
    }

    return canCraft;
}


    public bool TryCraftItem(Recipe recipe)
    {
        string inventoryContents;
        if (CheckRecipeIngredients(recipe, out inventoryContents))
        {
            Debug.Log("Item jest tworzony, sa skladniki");
            foreach (Ingredient ingredient in recipe.ingredients)
            {
                RemoveItem(ingredient.name, ingredient.quantity);
            }
            Debug.Log("Przedmiot zostal stworzony aby item dzialal");
            return true;
        }
        else
        {
            Debug.Log("Nie mozna stworzyc przedmiotu zamalo skladnikow");
            Debug.Log (inventoryContents);
            return false;
        }
    }
}
