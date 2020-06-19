using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuItemPrefab : MonoBehaviour
{
    public bool selected = false;
    public Color colorS;
    public Color colorD;

    public RadialMenu.RadialMenuItem item;

    

    private void Update()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = item.icon;
        if (selected)
        {
            
            GetComponent<UnityEngine.UI.Image>().color = colorS;
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().color = colorD;
        }
    }
}
