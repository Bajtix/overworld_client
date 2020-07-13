using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int count;

    public ItemStack(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public ItemStack(Item item)
    {
        this.item = item;
        this.count = 1;
    }
}
