using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 4;

    public InventorySlot[] inventorySlots;
    int selectedSlot =-1;

    public InventoryText inventoryText;
    public Chest chest;
    public GameObject inventoryItemPrefab;

    private void Start(){

    }

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
                      inventoryText.DisplayItemAddedMessage(item);
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
                      inventoryText.DisplayItemAddedMessage(item);
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

public bool CheckRecipeIngredients(Recipe recipe, out string inventoryContents, out Dictionary<string, int>? remainingChestItems, out Dictionary<string, int>? usedChestItems, out Dictionary<string, int>? missingItemsDict)
{
    // Inicjalizacja słownika do przechowywania ilości przedmiotów w skrzyni
    Dictionary<string, int> chestCounts = new Dictionary<string, int>();
    bool chestOpen = chest.Openchest();
    if (chestOpen)
    {
        // Pobieranie przedmiotów ze skrzyni
        chestCounts = chest.GetItemsFromChestWithCounts();
    }

    // Inicjalizacja słowników wynikowych
    remainingChestItems = new Dictionary<string, int>(chestCounts);
    usedChestItems = new Dictionary<string, int>();
    missingItemsDict = new Dictionary<string, int>();

    // Inicjalizacja słownika do przechowywania ilości przedmiotów w inwentarzu
    Dictionary<string, int> inventoryCounts = new Dictionary<string, int>();
    inventoryContents = "";
    bool canCraft = true;

    // Sprawdzanie zawartości skrzyni
    if (chestCounts.Count > 0)
    {
        inventoryContents += "Chest contents:\n";
        foreach (var item in chestCounts)
        {
            inventoryContents += $"{item.Key}: {item.Value}\n";
            Debug.Log($"{item.Key}: {item.Value}");
        }
    }
    else
    {
        inventoryContents += "The chest is empty.\n";
        Debug.Log("The chest is empty.");
    }

    // Inicjalizacja słownika z ilościami składników przepisu
    foreach (Ingredient ingredient in recipe.ingredients)
    {
        if (!inventoryCounts.ContainsKey(ingredient.name))
        {
            inventoryCounts[ingredient.name] = 0;
        }
    }

    // Pobieranie zawartości inwentarza
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

    // Sprawdzanie dostępności składników
    List<string> missingItems = new List<string>();
    foreach (Ingredient ingredient in recipe.ingredients)
    {
        int countNeeded = ingredient.quantity;
        int countAvailableFromInventory = inventoryCounts.ContainsKey(ingredient.name) ? inventoryCounts[ingredient.name] : 0;

        // Porównanie ilości w inwentarzu z ilością w przepisie
        if (countAvailableFromInventory > countNeeded)
        {
            int surplus = countAvailableFromInventory - countNeeded;
            inventoryContents += $"Inventory has more {ingredient.name} than needed by {surplus} units.\n";
            Debug.Log($"Inventory has more {ingredient.name} than needed by {surplus} units.");
        }
        else if (countAvailableFromInventory < countNeeded)
        {
            int deficit = countNeeded - countAvailableFromInventory;
            inventoryContents += $"Inventory has less {ingredient.name} than needed by {deficit} units.\n";
            Debug.Log($"Inventory has less {ingredient.name} than needed by {deficit} units.");
        }
        else
        {
            inventoryContents += $"Inventory has exactly the needed amount of {ingredient.name}.\n";
            Debug.Log($"Inventory has exactly the needed amount of {ingredient.name}.");
        }

        // Najpierw odejmujemy z inwentarza
        if (countAvailableFromInventory >= countNeeded)
        {
            inventoryCounts[ingredient.name] -= countNeeded;
            inventoryContents += $"Used {countNeeded} {ingredient.name} from inventory\n";
            Debug.Log($"Used {countNeeded} {ingredient.name} from inventory");
        }
        else
        {
            int remainingNeed = countNeeded - countAvailableFromInventory;

            // Odejmujemy z inwentarza, jeśli coś było dostępne
            if (countAvailableFromInventory > 0)
            {
                inventoryCounts[ingredient.name] = 0;
                inventoryContents += $"Used {countAvailableFromInventory} {ingredient.name} from inventory\n";
                Debug.Log($"Used {countAvailableFromInventory} {ingredient.name} from inventory");
            }

            // Sprawdzamy i odejmujemy resztę ze skrzyni
            if (remainingNeed > 0)
            {
                int countAvailableFromChest = chestCounts.ContainsKey(ingredient.name) ? chestCounts[ingredient.name] : 0;

                if (chestOpen && countAvailableFromChest >= remainingNeed)
                {
                    chestCounts[ingredient.name] -= remainingNeed;
                    remainingChestItems[ingredient.name] = chestCounts[ingredient.name];

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
                else if (chestOpen && countAvailableFromChest < remainingNeed)
                {
                    canCraft = false;
                    int missingAmount = remainingNeed - countAvailableFromChest;
                    missingItemsDict[ingredient.name] = missingAmount;
                    inventoryContents += $"Missing {missingAmount} {ingredient.name} from the chest.\n";
                    Debug.Log($"{ingredient.name}: Missing {missingAmount}");
                }
                else
                {
                    canCraft = false;
                    inventoryContents += $"Cannot craft because the chest is not open and {remainingNeed} {ingredient.name} is needed.\n";
                    Debug.Log($"Cannot craft because the chest is not open and {remainingNeed} {ingredient.name} is needed.");
                }
            }
        }
    }

    // Dodawanie informacji o brakujących składnikach
    if (missingItemsDict.Count > 0)
    {
        inventoryContents += "\nMissing items:\n";
        foreach (var missingItem in missingItemsDict)
        {
            inventoryContents += $"{missingItem.Key}: {missingItem.Value}\n";
            Debug.Log($"{missingItem.Key}: {missingItem.Value}");
        }
    }

    // Zwracanie wartości czy przepis może być wykonany
    return canCraft;
}



    public bool TryCraftItem(Recipe recipe)
    {
        string inventoryContents;
        Dictionary<string, int> usedChestItems;
        Dictionary<string, int>? remainingChestItems;
        Dictionary<string, int>? missingItemsDict;
        if (CheckRecipeIngredients(recipe, out inventoryContents, out remainingChestItems, out usedChestItems, out missingItemsDict))
        {   

            Debug.Log("Item jest tworzony, sa skladniki");
            foreach (Ingredient ingredient in recipe.ingredients)
            {
                RemoveItem(ingredient.name, ingredient.quantity);
               
            }
                           RemoveItemsFromChest(usedChestItems);

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