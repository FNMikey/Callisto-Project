using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crafting : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Open open;
    public Item item;

    public Chest chest;
    private Recipe pickaxeRockRecipe = new Recipe();
    private Recipe pickaxeIronRecipe = new Recipe();
    private Recipe axeRockRecipe = new Recipe();
    private Recipe axeIronRecipe = new Recipe();
    private Recipe goldFigureRecipe = new Recipe();
    private Recipe furnaceRecipe = new Recipe();
    public void CraftFurnace()
    {
        furnaceRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Iron", quantity = 8 }
        };
        bool success = inventoryManager.TryCraftItem(furnaceRecipe);
        if (success){
             if (inventoryManager.IsInventoryFull() && open.Openchest()) {
                    chest.AddItemToChest(item);
                                Debug.Log("Piec zostal stworzony w skrzynce");
            }else{
                    inventoryManager.AddItem(item);
                                Debug.Log("Piec zostal stworzony");
            }    
        }
        else
        {
            Debug.Log("Piec nie zostal stworzony");
        }
    }
    public void CraftGoldFigure()
    {
        goldFigureRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Gold", quantity = 5 }
        };
        bool success = inventoryManager.TryCraftItem(goldFigureRecipe);
        if (success){
             if (inventoryManager.IsInventoryFull() && open.Openchest()) {
                    chest.AddItemToChest(item);
                        Debug.Log("zlota figurka jest  w skrzynce");
            }else{
                    inventoryManager.AddItem(item);
                        Debug.Log("zlota figurka zostal stworzony");
            }
        }
        else
        {
            Debug.Log("zlota figurka nie zostala stworzony");
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
                if (inventoryManager.IsInventoryFull() && open.Openchest()) {
                    chest.AddItemToChest(item);
                                Debug.Log("Piec zostal Topor w skrzynce");
            }else{
                    inventoryManager.AddItem(item);
                                Debug.Log("Piec zostal topor");
            }
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
                 if (inventoryManager.IsInventoryFull() && open.Openchest()) {
                    chest.AddItemToChest(item);
                                Debug.Log("topor zostal stworzony w skrzynce");
            }else{
                    inventoryManager.AddItem(item);
                                Debug.Log("topor zostal stworzony");
            }
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
                 if (inventoryManager.IsInventoryFull() && open.Openchest()) {
                    chest.AddItemToChest(item);
                                Debug.Log("Kilof zostal stworzony w skrzynce");
            }else{
                    inventoryManager.AddItem(item);
                                Debug.Log("K<ilof zostal stworzony");
            }
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
              if (inventoryManager.IsInventoryFull() && open.Openchest()) {
                    chest.AddItemToChest(item);
                                Debug.Log("Kilof zostal stworzony w skrzynce");
            }else{
                    inventoryManager.AddItem(item);
                                Debug.Log("K<ilof zostal stworzony");
            }
        }
        else
        {
            Debug.Log("Kilof nie zostal stworzony");
        }
    }
}