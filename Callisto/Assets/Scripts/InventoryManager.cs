using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 4;

    public InventorySlot[] inventorySlots;

    public Chest chest;
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

public bool CheckRecipeIngredients(Recipe recipe, out string inventoryContents, out Dictionary<string, int>? remainingChestItems, out Dictionary<string, int>? usedChestItems)
{
    Dictionary<string, int> chestCounts = new Dictionary<string, int>();
    if (chest.Openchest()) // Assuming you have a method to check if the chest is open
    {
        chestCounts = chest.GetItemsFromChestWithCounts();
    }

    remainingChestItems = new Dictionary<string, int>(chestCounts);
    usedChestItems = new Dictionary<string, int>();

    Dictionary<string, int> inventoryCounts = new Dictionary<string, int>();
    inventoryContents = "";
    bool canCraft = true;

    // Display chest contents
    if (chestCounts.Count > 0)
    {
        inventoryContents += "Chest contents:\n";
        foreach (var item in chestCounts)
        {
            inventoryContents += $"{item.Key}: {item.Value}\n";
            Debug.Log($"{item.Key}: {item.Value}"); // Display in console
        }
    }
    else
    {
        inventoryContents += "The chest is empty.\n";
        Debug.Log("The chest is empty."); // Display in console
    }

    // Initialize counter for each recipe ingredient
    foreach (Ingredient ingredient in recipe.ingredients)
    {
        if (!inventoryCounts.ContainsKey(ingredient.name))
        {
            inventoryCounts[ingredient.name] = 0;
        }
    }

    // Count items in inventory
    for (int i = 0; i < inventorySlots.Length; i++)
    {
        InventorySlot slot = inventorySlots[i];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            if (inventoryCounts.ContainsKey(itemInSlot.item.itemName))
            {
                inventoryCounts[itemInSlot.item.itemName] += itemInSlot.count;
            }

            if (itemInSlot.item.stackable)
            {
                inventoryContents += $"Slot {i}: {itemInSlot.item.itemName}, Quantity: {itemInSlot.count}\n";
            }
            else
            {
                inventoryContents += $"Slot {i}: {itemInSlot.item.itemName}\n";
            }
        }
        else
        {
            inventoryContents += $"Slot {i}: Empty\n";
        }
    }

    List<string> missingItems = new List<string>();

    // Check availability of ingredients
    foreach (Ingredient ingredient in recipe.ingredients)
    {
        int countNeeded = ingredient.quantity;
        int countAvailableFromInventory = inventoryCounts.ContainsKey(ingredient.name) ? inventoryCounts[ingredient.name] : 0;
        int countAvailableFromChest = chestCounts.ContainsKey(ingredient.name) ? chestCounts[ingredient.name] : 0;
        int totalAvailable = countAvailableFromInventory + countAvailableFromChest;

        inventoryContents += $"{ingredient.name}: Needed {countNeeded}, Available {totalAvailable} (Inventory: {countAvailableFromInventory}, Chest: {countAvailableFromChest})\n";

        if (totalAvailable < countNeeded)
        {
            canCraft = false;
            missingItems.Add($"{ingredient.name}: Missing {countNeeded - totalAvailable}");
        }
    }

    // Only deduct items if all ingredients are available in sufficient quantities
    if (canCraft)
    {
        foreach (Ingredient ingredient in recipe.ingredients)
        {
            int countNeeded = ingredient.quantity;
            int countAvailableFromInventory = inventoryCounts.ContainsKey(ingredient.name) ? inventoryCounts[ingredient.name] : 0;
            int countAvailableFromChest = chestCounts.ContainsKey(ingredient.name) ? chestCounts[ingredient.name] : 0;

            // First deduct from inventory
            if (countAvailableFromInventory >= countNeeded)
            {
                inventoryCounts[ingredient.name] -= countNeeded;
                inventoryContents += $"Used {countNeeded} {ingredient.name} from inventory\n";
            }
            else if (countAvailableFromChest >= countNeeded)
            {
                // Then deduct from chest
                chestCounts[ingredient.name] -= countNeeded;
                remainingChestItems[ingredient.name] = chestCounts[ingredient.name]; // Update remaining chest items

                if (usedChestItems.ContainsKey(ingredient.name))
                {
                    usedChestItems[ingredient.name] += countNeeded;
                }
                else
                {
                    usedChestItems[ingredient.name] = countNeeded;
                }

                inventoryContents += $"Used {countNeeded} {ingredient.name} from the chest\n";
                Debug.Log($"Used {countNeeded} {ingredient.name} from the chest. Remaining {chestCounts[ingredient.name]} in the chest.");
            }
            else if (countAvailableFromInventory + countAvailableFromChest >= countNeeded)
            {
                // If needed quantity is not fully available from inventory or chest individually, but combined they are enough
                int remainingNeed = countNeeded - countAvailableFromInventory;

                if (countAvailableFromInventory > 0)
                {
                    inventoryCounts[ingredient.name] = 0;
                    inventoryContents += $"Used {countAvailableFromInventory} {ingredient.name} from inventory\n";
                }

                chestCounts[ingredient.name] -= remainingNeed;
                remainingChestItems[ingredient.name] = chestCounts[ingredient.name]; // Update remaining chest items

                if (usedChestItems.ContainsKey(ingredient.name))
                {
                    usedChestItems[ingredient.name] += remainingNeed;
                }
                else
                {
                    usedChestItems[ingredient.name] = remainingNeed;
                }

                inventoryContents += $"Used {remainingNeed} {ingredient.name} from the chest\n";
                Debug.Log($"Used {remainingNeed} {ingredient.name} from the chest. Remaining {chestCounts[ingredient.name]} in the chest.");
            }
            else
            {
                canCraft = false;
                missingItems.Add($"{ingredient.name}: Missing {countNeeded - (countAvailableFromInventory + countAvailableFromChest)}");
            }
        }
    }

    // Display missing items in console
    if (missingItems.Count > 0)
    {
        inventoryContents += "\nMissing items:\n";
        foreach (string missingItem in missingItems)
        {
            inventoryContents += missingItem + "\n";
            Debug.Log(missingItem); // Display in console
        }
    }

    return canCraft;
}





    public bool TryCraftItem(Recipe recipe)
    {
        string inventoryContents;
        Dictionary<string, int> usedChestItems;
        Dictionary<string, int>? remainingChestItems;
        if (CheckRecipeIngredients(recipe, out inventoryContents, out remainingChestItems, out usedChestItems))
        {   

            Debug.Log("Item jest tworzony, sa skladniki");
            foreach (Ingredient ingredient in recipe.ingredients)
            {
                RemoveItem(ingredient.name, ingredient.quantity);
                RemoveItemsFromChest(remainingChestItems);

                
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
     public void RemoveItemsFromChest(Dictionary<string, int> itemsToRemove)
    {
        foreach (var item in itemsToRemove)
        {
            chest.RemoveItemChest(item.Key, item.Value);
            Debug.Log("To jest" + item.Key + "" + item.Value + "");
        }
    }

    }