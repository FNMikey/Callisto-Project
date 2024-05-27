using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crafting : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item item;
    private Recipe pickaxeRockRecipe = new Recipe();
    private Recipe pickaxeIronRecipe = new Recipe();
    private Recipe axeRockRecipe = new Recipe();
    private Recipe axeIronRecipe = new Recipe();
    private Recipe swordIronRecipe = new Recipe();
    private Recipe furnaceRecipe = new Recipe();
    public void CraftFurnace()
    {
        furnaceRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Iron", quantity = 8 }
        };
        bool success = inventoryManager.TryCraftItem(furnaceRecipe);
        if (success)
        {
            Debug.Log("Piec zostal stworzony");
            inventoryManager.AddItem(item);
        }
        else
        {
            Debug.Log("Piec nie zostal stworzony");
        }
    }
    public void CraftSwordIron()
    {
        swordIronRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 3 },
            new Ingredient { name = "Iron", quantity = 6 }
        };
        bool success = inventoryManager.TryCraftItem(swordIronRecipe);
        if (success)
        {
            Debug.Log("Miecz zostal stworzony");
            inventoryManager.AddItem(item);
        }
        else
        {
            Debug.Log("Miecz nie zostal stworzony");
        }
    }
    public void CraftRockAxe()
    {
        axeRockRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "Rock", quantity = 3 }
        };
        bool success = inventoryManager.TryCraftItem(axeRockRecipe);
        if (success)
        {
            Debug.Log("Siekiera zostal stworzony");
            inventoryManager.AddItem(item);
        }
        else
        {
            Debug.Log("Siekiera nie zostal stworzony");
        }
    }
    public void CraftIronAxe()
    {
        axeIronRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "Iron", quantity = 3 }
        };
        bool success = inventoryManager.TryCraftItem(axeIronRecipe);
        if (success)
        {
            Debug.Log("Siekiera zostal stworzony");
            inventoryManager.AddItem(item);
        }
        else
        {
            Debug.Log("Siekiera nie zostal stworzony");
        }
    }
    public void CraftIronPickaxe()
    {
        pickaxeIronRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "Iron", quantity = 3 }
        };
        bool success = inventoryManager.TryCraftItem(pickaxeIronRecipe);
        if (success)
        {
            Debug.Log("Kilof zostal stworzony");
            inventoryManager.AddItem(item);
        }
        else
        {
            Debug.Log("Kilof nie zostal stworzony");
        }
    }

    public void CraftRockPickaxe()
    {
        pickaxeRockRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "Rock", quantity = 3 }
        };
        bool success = inventoryManager.TryCraftItem(pickaxeRockRecipe);
        if (success)
        {
            Debug.Log("Kilof zostal stworzony");
            inventoryManager.AddItem(item);
        }
        else
        {
            Debug.Log("Kilof nie zostal stworzony");
        }
    }
}