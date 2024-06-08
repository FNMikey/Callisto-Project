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
        Dictionary<string, int>? missingItemsDict;

        bool canCraft = inventoryManager.CheckRecipeIngredients(recipe, out details, out remainingChestItems, out usedChestItems, out missingItemsDict);
        if (canCraft)
        {
            Debug.Log("Recipe can be crafted.");
        }
        else
        {
            Debug.Log("Recipe cannot be crafted.");
            Debug.Log(details);
        }

        return canCraft;
    }

    public void PickupItem(Item itemToPickup)
    {
        if (itemToPickup == null)
        {
            return;
        }

        if (inventoryManager == null)
        {
            return;
        }

        if (inventoryManager.IsInventoryFull())
        {
            Debug.Log("Ekwipunek jest pelen");
            return;
        }

        if (inventoryManager.AddItem(itemToPickup))
        {
            Debug.Log($"{itemToPickup.itemName} zostal dodany.");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"{itemToPickup.itemName} nie zostal dodany.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (item != null)
            {
                PickupItem(item);
            }
        }
    }
}
