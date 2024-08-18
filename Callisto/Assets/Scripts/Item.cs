using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemID;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool stackable = true;
    public Sprite image;
    public GameObject prefab; // Add this line to include a reference to the item's prefab
}


public enum ItemType
{
    BuildingBlock,
    Tool
}

public enum ActionType
{
    Dig,
    Build,
    Cut
}
