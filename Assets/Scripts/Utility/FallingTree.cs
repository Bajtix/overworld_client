using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
public class FallingTree : MonoBehaviour
{
    public GameObject[] trees;
    private bool set = false;
    private void Update()
    {
        if (!set)
        {
            if (GetComponent<Entity>().additionalData != "")
            {
                int mid = int.Parse(GetComponent<Entity>().additionalData);
                Instantiate(trees[mid], transform);
                set = true;
            }
        }
    }
}
