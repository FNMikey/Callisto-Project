using UnityEngine;
using System.Collections.Generic;

public class Chest : MonoBehaviour
{
    public Transform player;  
        public InventoryManager inventoryMenager;
    public GameObject chestUI;  
    public float interactionDistance = 3f;  
    private bool isOpen = false; 

    public InventorySlot[] chestSlots; 
    private List<InventoryItem> chestItems = new List<InventoryItem>();

    void Start()
    {
        if (chestUI != null)
        {
            chestUI.SetActive(false); 
        }
    }

    void Update()
    {
        bool open = Openchest();
        if (IsPlayerNearby())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleChest();
                
            }
        }
        else if (open)
        {
            CloseChest();            
        }
    }

    public bool IsPlayerNearby()
    {
        return Vector3.Distance(player.position, transform.position) <= interactionDistance;
    }

    private void ToggleChest()
    {
        isOpen = !isOpen; 
        if (isOpen)
        {
            Debug.Log("Chest opened!");
        }
        else
        {
            Debug.Log("Chest closed!");
        }
        UpdateChestUI();
    }

    private void CloseChest()
    {
        isOpen = false;
        UpdateChestUI();
        Debug.Log("Chest closed due to distance!");
    }

    private void UpdateChestUI()
    {
        if (isOpen)
        {
            if (chestUI != null)
            {
                chestUI.SetActive(true); 
            }
        }
        else
        {
            if (chestUI != null)
            {
                chestUI.SetActive(false); 
            }
        }
    }

public Dictionary<string, int> GetItemsFromChestWithCounts()
{
    Dictionary<string, int> itemCounts = new Dictionary<string, int>();  

    if (chestSlots == null)
    {
        Debug.LogWarning("Miejsce jest puste");
        return itemCounts;
    }

    for (int i = 0; i < chestSlots.Length; i++)
    {
        InventorySlot slot = chestSlots[i];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null && itemInSlot.item != null)
        {
            string itemName = itemInSlot.item.itemName;
            if (itemCounts.ContainsKey(itemName))
            {
                itemCounts[itemName] += itemInSlot.count;
            }
            else
            {
                itemCounts[itemName] = itemInSlot.count;
            }

            Debug.Log($"Miejsce {i}: {itemName}, Ilosc: {itemInSlot.count}");
        }
        else
        {
            Debug.Log($"Miejsce {i} jest puste");
        }
    }
    foreach (var item in itemCounts)
    {
        Debug.Log($"Przedmiot: {item.Key}, Ilosc: {item.Value}");
    }

    return itemCounts;  
}

    public bool Openchest(){
        if(chestUI.activeSelf){
                        Debug.Log("Chest is opnen");
                  return true;
        }else{
              //Debug.Log("Chest is close");
            return false;
        }
    }
    
    public bool RemoveItemChest(string itemName, int quantity)
    {
        int remainingQuantity = quantity;

        foreach (InventorySlot slot in chestSlots)
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

    
    public bool AddItemToChest(Item item)
    {
        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (
                itemInSlot != null &&
                itemInSlot.item == item 
            )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        for (int i = 0; i < chestSlots.Length; i++)
        {
            InventorySlot slot = chestSlots[i];
            InventoryItem itemInSlot =
                slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                inventoryMenager.SpawnNewItem (item, slot);
                return true;
            }
        }
        return false;
    }


}
