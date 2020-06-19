using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Holdable Item", menuName = "Game/Item/Holdable")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public new string name;
    public string description;
    public int maxStackCount;
    public GameObject model;
}
