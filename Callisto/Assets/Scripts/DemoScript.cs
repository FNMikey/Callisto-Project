using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Item[] itemsToPickup;

    public Item item;

    private bool TryCraftingRecipe(Recipe recipe)
    {
        string details;
        Dictionary<string, int> usedChestItems;
        Dictionary<string, int>? remainingChestItems;
        bool craft =
            inventoryManager.CheckRecipeIngredients(recipe, out details, out remainingChestItems, out usedChestItems);
        if (craft)
        {
            Debug.Log("Mozna stworzyc kilofa");
            return craft;
        }
        else
        {
            Debug.Log("Nie mozna stworzyc kilofa");
            Debug.Log (details);
            return craft;
        }
    }
    public void PickupItem(Item item)
{
    if (item == null) {
        Debug.LogError("Item is null");
        return;
    }
    if (inventoryManager == null) {
        Debug.LogError("InventoryManager is not set");
        return;
    }
    if (inventoryManager.IsInventoryFull()) {
    } else {
        if (inventoryManager.AddItem(item)) {
            Debug.Log($"{item.itemName} has been added");
            gameObject.SetActive(false);
        } else {
            Debug.Log($"{item.itemName} has not been added");
        }
    }
}

private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player")) {
        if (item != null) {
                PickupItem(item);
        } else {
            Debug.LogError("Item not set for trigger");
        }
    }
}
}
