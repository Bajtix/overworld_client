using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public TMPro.TextMeshProUGUI count;
    public int id;

    
    public void SetNull()
    {
        image.sprite = null;
        count.text = "";
    }

    public void SetItem(ItemStack stack)
    {
        image.sprite = stack.item.icon;
        count.text = stack.count.ToString("000");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("SlotClicked");
        UIManager.instance.GetComponent<InventoryRenderer>().ToggleTransfer(id);
        UIManager.instance.GetComponent<InventoryRenderer>().HideInventory();
        ClientSend.InventoryRequest();
        
    }
}
