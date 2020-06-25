using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public GameManager._EDIC display;
    private Entity e;

    private void Start()
    {
        e = GetComponent<Entity>();
    }
    private void Update()
    {
        string sel = e.additionalData;
        foreach(GameObject g in display.Values)
        {
            g.SetActive(false);
        }
        if(display.ContainsKey(sel))
            display[sel].SetActive(true);
    }
}
