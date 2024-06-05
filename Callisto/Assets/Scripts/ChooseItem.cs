using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseItem : MonoBehaviour
{
    int selectedSlot = -1;

    public Color selectedColor = Color.yellow;
    public Color notSelectedColor = Color.white;

    public InventorySlot[] inventorySlots;

    private void Start()
    {
        if (inventorySlots.Length > 0)
        {
            ChangeSelectedSlot(0);
        }
    }

    void Update()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Sprawdź, który przycisk numeryczny jest naciskany
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeSelectedSlot(i);
            }
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0 && selectedSlot < inventorySlots.Length)
        {
            inventorySlots[selectedSlot].Deselect();
        }

        if (newValue >= 0 && newValue < inventorySlots.Length)
        {
            inventorySlots[newValue].Select();
            selectedSlot = newValue;
        }
    }
}
