using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameRenderer : MonoBehaviour
{
    public GameObject textPrefab;
    public TextMeshProUGUI nametag;
    private void Start()
    {
        nametag = Instantiate(textPrefab,UIManager.instance.transform).GetComponent<TextMeshProUGUI>();
        nametag.text = GetComponent<PlayerManager>().username;
        
    }

    private void Update()
    {
        nametag.transform.position = Camera.current.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 0));
        nametag.GetComponent<CanvasGroup>().alpha = 2 - Vector3.Distance(transform.position, Camera.current.transform.position) * 0.5f;
    }
}
