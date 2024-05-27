using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Recipe : MonoBehaviour
{
    public List<Ingredient> ingredients;

    public Item result;
}

[System.Serializable]
public class Ingredient
{
    public string name;

    public int quantity;
}
