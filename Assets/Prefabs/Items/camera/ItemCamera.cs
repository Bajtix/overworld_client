using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCamera : ItemReactor
{
    public Camera lens;
    public RenderTexture renderTexture;
    public MeshRenderer screen;
    public Material baseMaterial;

    public GameObject flash;

    private void Start()
    {
        renderTexture = new RenderTexture(128, 128, 8);
        
        lens.targetTexture = renderTexture;
        renderTexture.Create();
        Material m = Instantiate(baseMaterial);
        m.mainTexture = renderTexture;
        
        screen.material = m;
    }

    public override void FPSResponse(int response)
    {
        gameObject.SetActive(false);
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Capture" + DateTime.Now.ToString().Replace(':', '-') + ".png");
        LeanTween.delayedCall(0.2f, () => gameObject.SetActive(true));
        

    }

    public override void TPSResponse(int response)
    {
        flash.SetActive(true);
        LeanTween.delayedCall(0.1f, () => flash.SetActive(false));
    }


}
