using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item item;
    public Chest chest;

    public InventoryText inventoryText;
    private Recipe pickaxeRockRecipe = new Recipe();
    private Recipe pickaxeIronRecipe = new Recipe();
    private Recipe axeRockRecipe = new Recipe();
    private Recipe axeIronRecipe = new Recipe();
    private Recipe goldFigureRecipe = new Recipe();
    private Recipe furnaceRecipe = new Recipe();
    private Recipe trophyRecipe= new Recipe();

    void Start()
    {
        InitializeRecipes();
    }

    private void InitializeRecipes()
    {
        pickaxeRockRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "Rock", quantity = 2 }
        };

        pickaxeIronRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "IronBar", quantity = 3 }
        };

        axeRockRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "Rock", quantity = 3 }
        };

        axeIronRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Stick", quantity = 2 },
            new Ingredient { name = "IronBar", quantity = 3 }
        };

        goldFigureRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Gold", quantity = 5 }
        };

        furnaceRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "Iron", quantity = 8 }
        };

        trophyRecipe.ingredients = new List<Ingredient>
        {
            new Ingredient { name = "GoldBar", quantity = 3 }
        };
    }

    public void StartCreatingItem()
    {
        StartCoroutine(CreateItem());
    }

    private IEnumerator CreateItem()
    {
        for (int i = 10; i >= 1; i--)
        {
            Debug.Log($"Counting: {i}");
            inventoryText.DisplayCounterMessage(item, i);
            yield return new WaitForSeconds(2);
        }
        CompleteCrafting();
    }

    private void CompleteCrafting()
    {
        if (inventoryManager.IsInventoryFull() && chest.Openchest())
        {
            chest.AddItemToChest(item);
            inventoryText.DisplayItemCreateMessage(item);
        }
        else
        {
            inventoryManager.AddItem(item);
            inventoryText.DisplayItemCreateMessage(item);
        }
    }

    public void CraftItem(Recipe recipe)
    {
        bool success = inventoryManager.TryCraftItem(recipe);
        if (success)
        {
            StartCreatingItem();
        }
        else
        {
            inventoryText.DisplayItemNotCreateMessage(item);
        }
    }

    public void CraftFurnace()
    {
        //CraftItem(furnaceRecipe);
        CraftItem(trophyRecipe);
    }

    public void CraftTrophy()
    {
        CraftItem(trophyRecipe);
    }

    public void CraftGoldFigure()
    {
        CraftItem(goldFigureRecipe);
    }

    public void CraftRockAxe()
    {
        CraftItem(axeRockRecipe);
    }

    public void CraftIronAxe()
    {
        CraftItem(axeIronRecipe);
    }

    public void CraftIronPickaxe()
    {
        CraftItem(pickaxeIronRecipe);
    }

    public void CraftRockPickaxe()
    {
        CraftItem(pickaxeRockRecipe);
    }
}
