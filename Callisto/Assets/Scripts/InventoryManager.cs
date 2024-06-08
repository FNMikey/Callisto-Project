using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public InventoryText inventoryText;
    public Chest chest;
    public GameObject inventoryItemPrefab;

    private int selectedSlot = -1;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeSelectedSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeSelectedSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeSelectedSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeSelectedSlot(3);
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0) inventorySlots[selectedSlot].Deselect();
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool RemoveItem(string itemName, int quantity)
    {
        int remainingQuantity = quantity;

        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.name == itemName)
            {
                if (itemInSlot.count >= remainingQuantity)
                {
                    itemInSlot.count -= remainingQuantity;
                    if (itemInSlot.count == 0) Destroy(itemInSlot.gameObject);
                    itemInSlot.RefreshCount();
                    return true;
                }
                else
                {
                    remainingQuantity -= itemInSlot.count;
                    Destroy(itemInSlot.gameObject);
                }
            }
        }

        return false;
    }

    public bool IsInventoryFull()
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null || (itemInSlot.count < maxStackedItems && itemInSlot.item.stackable)) return false;
        }
        return true;
    }

    public bool AddItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                //inventoryText.DisplayItemAddedMessage(item);
                return true;
            }
        }

        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                //inventoryText.DisplayItemAddedMessage(item);
                return true;
            }
        }

        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public bool CheckRecipeIngredients(Recipe recipe, out string inventoryContents, out Dictionary<string, int> remainingChestItems, out Dictionary<string, int> usedChestItems, out Dictionary<string, int> missingItemsDict)
    {
        remainingChestItems = chest.Openchest() ? chest.GetItemsFromChestWithCounts() : new Dictionary<string, int>();
        usedChestItems = new Dictionary<string, int>();
        missingItemsDict = new Dictionary<string, int>();

        Dictionary<string, int> inventoryCounts = GetInventoryCounts();
        inventoryContents = BuildInventoryContents(inventoryCounts, remainingChestItems);

        return VerifyAndUpdateCounts(recipe, inventoryCounts, remainingChestItems, usedChestItems, missingItemsDict, ref inventoryContents);
    }

    private Dictionary<string, int> GetInventoryCounts()
    {
        Dictionary<string, int> inventoryCounts = new Dictionary<string, int>();

        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                if (inventoryCounts.ContainsKey(itemInSlot.item.itemName))
                {
                    inventoryCounts[itemInSlot.item.itemName] += itemInSlot.count;
                }
                else
                {
                    inventoryCounts[itemInSlot.item.itemName] = itemInSlot.count;
                }
            }
        }

        return inventoryCounts;
    }

    private string BuildInventoryContents(Dictionary<string, int> inventoryCounts, Dictionary<string, int> chestCounts)
    {
        string inventoryContents = "Inventory contents:\n";

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            inventoryContents += itemInSlot != null
                ? $"Slot {i}: {itemInSlot.item.itemName}, Quantity: {itemInSlot.count}\n"
                : $"Slot {i}: Empty\n";
        }

        inventoryContents += "Chest contents:\n";
        foreach (var item in chestCounts)
        {
            inventoryContents += $"{item.Key}: {item.Value}\n";
        }

        return inventoryContents;
    }

    private bool VerifyAndUpdateCounts(Recipe recipe, Dictionary<string, int> inventoryCounts, Dictionary<string, int> remainingChestItems, Dictionary<string, int> usedChestItems, Dictionary<string, int> missingItemsDict, ref string inventoryContents)
    {
        bool canCraft = true;

        foreach (var ingredient in recipe.ingredients)
        {
            int countNeeded = ingredient.quantity;
            int countAvailableFromInventory = inventoryCounts.GetValueOrDefault(ingredient.name, 0);
            int countAvailableFromChest = remainingChestItems.GetValueOrDefault(ingredient.name, 0);

            if (countAvailableFromInventory >= countNeeded)
            {
                inventoryCounts[ingredient.name] -= countNeeded;
                inventoryContents += $"Used {countNeeded} {ingredient.name} from inventory\n";
            }
            else
            {
                int remainingNeed = countNeeded - countAvailableFromInventory;
                inventoryCounts[ingredient.name] = 0;
                inventoryContents += $"Used {countAvailableFromInventory} {ingredient.name} from inventory\n";

                if (remainingNeed <= countAvailableFromChest)
                {
                    remainingChestItems[ingredient.name] -= remainingNeed;
                    usedChestItems[ingredient.name] = usedChestItems.GetValueOrDefault(ingredient.name, 0) + remainingNeed;
                    inventoryContents += $"Used {remainingNeed} {ingredient.name} from the chest\n";
                }
                else
                {
                    canCraft = false;
                    int missingAmount = remainingNeed - countAvailableFromChest;
                    missingItemsDict[ingredient.name] = missingAmount;
                    inventoryContents += $"Missing {missingAmount} {ingredient.name} from the chest\n";
                }
            }
        }

        if (missingItemsDict.Count > 0)
        {
            inventoryContents += "\nMissing items:\n";
            foreach (var missingItem in missingItemsDict)
            {
                inventoryContents += $"{missingItem.Key}: {missingItem.Value}\n";
            }
        }

        return canCraft;
    }

    public bool TryCraftItem(Recipe recipe)
    {
        if (CheckRecipeIngredients(recipe, out string inventoryContents, out var remainingChestItems, out var usedChestItems, out var missingItemsDict))
        {
            foreach (var ingredient in recipe.ingredients)
            {
                RemoveItem(ingredient.name, ingredient.quantity);
            }

            RemoveItemsFromChest(usedChestItems);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItemsFromChest(Dictionary<string, int> itemsToRemove)
    {
        foreach (var item in itemsToRemove)
        {
            chest.RemoveItemChest(item.Key, item.Value);
        }
    }
}
