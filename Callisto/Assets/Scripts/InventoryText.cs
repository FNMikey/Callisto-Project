using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Dodane, aby użyć IEnumerator

public class InventoryText : MonoBehaviour
{
    public Text itemAddedText;

    public void DisplayItemAddedMessage(Item item)
    {
        itemAddedText.text = $"Dodano {item.name} do ekwipunku!";
        StartCoroutine(ClearItemAddedMessage());
    }

    private IEnumerator ClearItemAddedMessage()
    {
        yield return new WaitForSeconds(2);
        itemAddedText.text = "";
    }
}
