using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public TMPro.TextMeshProUGUI count;

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
}
