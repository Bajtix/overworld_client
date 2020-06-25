using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [System.Serializable]
    public class RadialMenuItem
    {
        public string label;
        public Sprite icon;
        
        public UnityEngine.Events.UnityEvent call;
    }
    public Color selectedColor;
    public Color deselectedColor;
    public RadialMenuItem[] items;

    private List<Vector3> splitters;
    private List<RadialMenuItemPrefab> instances;

    public GameObject prefab;
    public GameObject canvas;

    public TextMeshProUGUI text;

    private float centerOffset = 10;
    public float referenceOffset = 400;

    public string menuName;

    private void OnEnable()
    {
        splitters = new List<Vector3>();
        instances = new List<RadialMenuItemPrefab>();
        ShowItems();
        
    }
    private void OnDisable()
    {
        HideItem();
    }

    

    public void ShowItems()
    {
        centerOffset = referenceOffset * (Screen.width / 1920f);
        Debug.Log("Screen width: " + Screen.width + " / 1920 = " + (referenceOffset * (Screen.width / 1920f)));
        float angleBetweenElements = 360f / items.Length;
        Vector3 spawnPos = new Vector3(0, 1, 0);
        foreach (RadialMenuItem i in items)
        {
            spawnPos = RotateVector2D(spawnPos, angleBetweenElements);
            splitters.Add(spawnPos);
            RadialMenuItemPrefab p = Instantiate(prefab, transform, false).AddComponent<RadialMenuItemPrefab>();
            
            p.transform.position = RotateVector2D(spawnPos, angleBetweenElements / 2) * centerOffset + transform.position;
            p.colorS = selectedColor;
            p.colorD = deselectedColor;
            p.item = i;
            instances.Add(p);
        }
    }

    public void HideItem()
    {
        foreach (RadialMenuItemPrefab i in instances)
        {
            Destroy(i.gameObject);
        }

        instances.Clear();
        splitters.Clear();
    }

    private void Update()
    {
        if (splitters.Count == 0) return;

        for (int i = 0; i < splitters.Count; i++)
        {
            instances[i].selected = false;
            Vector3 V1 = splitters[i];
            Vector3 V2 = i + 1 < splitters.Count ? splitters[i+1] : splitters[0];
            float angleArea = Vector3.Angle(V1, V2);
            if (Vector3.Angle(V1, Input.mousePosition - transform.position) < angleArea && Vector3.Angle(V2, Input.mousePosition - transform.position) < angleArea)
            {
                //Debug.Log("Selected " + i);
                instances[i].selected = true;
                text.text = items[i].label;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    items[i].call.Invoke();
                    Debug.Log("Menu called");
                }
            }
        }
    }


    private Vector3 RotateVector2D(Vector3 oldDirection, float angle)
    {
        float newX = Mathf.Cos(angle * Mathf.Deg2Rad) * (oldDirection.x) - Mathf.Sin(angle * Mathf.Deg2Rad) * (oldDirection.y);
        float newY = Mathf.Sin(angle * Mathf.Deg2Rad) * (oldDirection.x) + Mathf.Cos(angle * Mathf.Deg2Rad) * (oldDirection.y);
        float newZ = oldDirection.z;
        return new Vector3(newX, newY, newZ);
    }


    public void SendChosenOption(int opt)
    {
        ClientSend.SendMenuReply(opt,menuName);
    }
}
