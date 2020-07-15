using System.Collections.Generic;
using UnityEngine;

public class InventoryRenderer : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject panel;
    public GameObject invpanel;
    public Vector2 slotDimensions = new Vector2(100, 100);
    public Vector2 areaStart;
    public Vector2 slotCounts;

    public int from = -1, to = - 1;
    public bool transfering = false;


    private List<GameObject> allSlots;

    
    //to trzeba będzie przeprojektować na pewno. potrzebny będzie modularny ekwipunek, umożliwiający transfer między dwoma ekwipunkami.
    public void RenderInventory(ItemStack[] stacks)
    {
        Cursor.lockState = CursorLockMode.Confined;
        allSlots = new List<GameObject>();
        invpanel.SetActive(true);
        Vector2 _slotDimensions = slotDimensions * (Screen.width / 1920f);
        Vector2 _areaStart = areaStart * (Screen.width / 1920f);
        int c = 0;
        for (float x = 0; x < slotCounts.x; x++)
        {
            for (float y = 0; y < slotCounts.y; y++)
            {
                InventorySlot slot = Instantiate(slotPrefab, new Vector3(x * _slotDimensions.x + _areaStart.x, y * _slotDimensions.y + _areaStart.y), Quaternion.identity, panel.transform).GetComponent<InventorySlot>();
                slot.id = c;
                if (c < stacks.Length)
                {
                    if (stacks[c] != null)
                    {
                        slot.SetItem(stacks[c]);
                    }
                    else
                    {
                        slot.SetNull();
                    }
                }
                else
                {
                    slot.SetNull();
                }
                allSlots.Add(slot.gameObject);
                c++;
            }
        }
    }

    private void Start()
    {
        //RenderFakeInventory();
    }

    public void ToggleTransfer(int id)
    {
        if(transfering)
        {
            to = id;
            ClientSend.ItemTransfer(from,to);
            transfering = false;
        }
        else
        {
            transfering = true;
            from = id;
        }

    }

    public void RenderFakeInventory()
    {
        allSlots = new List<GameObject>();
        invpanel.SetActive(true);
        Vector2 _slotDimensions = slotDimensions * (Screen.width / 1920f);
        Vector2 _areaStart = areaStart * (Screen.width / 1920f);
        int c = 0;
        for (float x = 0; x < slotCounts.x; x++)
        {
            for (float y = 0; y < slotCounts.y; y++)
            {
                InventorySlot slot = Instantiate(slotPrefab, new Vector3(x * _slotDimensions.x + _areaStart.x, y * _slotDimensions.y + _areaStart.y), Quaternion.identity, panel.transform).GetComponent<InventorySlot>();
                slot.SetNull();
                allSlots.Add(slot.gameObject);
                c++;
            }
        }
    }

    public void HideInventory()
    {
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject g in allSlots)
            Destroy(g);
        invpanel.SetActive(false);
    }
}
