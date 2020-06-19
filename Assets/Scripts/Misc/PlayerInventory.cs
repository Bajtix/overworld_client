using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items;
    public Transform arm;
    public GameObject instance;

    public void SelectItem(string itemname)
    {
        if (instance != null) Destroy(instance);

        Item selectedItem = null;
        foreach(Item i in items)
        {
            if (i.name == itemname)
                selectedItem = i;
        }
        if(selectedItem == null) { Debug.LogWarning("Selected item was null"); return; }

        instance = Instantiate(selectedItem.model, arm);
    }
}
