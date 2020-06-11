using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public GameObject[] display;
    private Entity e;

    private void Start()
    {
        e = GetComponent<Entity>();
    }
    private void Update()
    {
        int sel = int.Parse(e.additionalData);
        for(int i = 0; i < display.Length; i++)
        {
            if (display[i] != null)
            {
                if (i != sel)
                    display[i].SetActive(false);
                else
                    display[i].SetActive(true);
            }
        }
    }
}
