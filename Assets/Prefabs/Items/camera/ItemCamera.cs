using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCamera : MonoBehaviour
{
    public Camera lens;
    public RenderTexture renderTexture;
    public MeshRenderer screen;
    public Material baseMaterial;

    private void Start()
    {
        renderTexture = new RenderTexture(128, 128, 8);
        
        lens.targetTexture = renderTexture;
        renderTexture.Create();
        //screen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", renderTexture);
        Material m = Instantiate(baseMaterial);
        m.mainTexture = renderTexture;
        
        screen.material = m;
        Debug.LogWarning("Set Material");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            gameObject.SetActive(false);
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Capture" + DateTime.Now.ToString().Replace(':','-') + ".png");
        }
    }

}
