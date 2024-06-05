using System;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public InventorySlot chestSlot;

    // This method will be called when the script is enabled
    void Start()
    {
        CheckForGoldenFigure();
    }

    // Method to check if the Golden Figure is in the chest slot
    private void CheckForGoldenFigure()
    {
        if (chestSlot == null)
        {
            Debug.LogWarning("Chest slot is not assigned.");
            return;
        }

        InventoryItem itemInSlot = chestSlot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null && itemInSlot.item != null)
        {
            if (itemInSlot.item.itemName == "GoldenFigure")
            {
                Debug.Log("Figurka jest w slocie");
            }
            else
            {
                Debug.Log("Figurka nie znajduje się w slocie.");
            }
        }
        else
        {
            Debug.Log("Slot jest pusty lub nie zawiera przedmiotu.");
        }
    }
}
