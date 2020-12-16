using System;
using UnityEngine;

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
        GameObject g = GameObject.FindObjectOfType<Canvas>().gameObject;
        gameObject.SetActive(false);
        g.SetActive(false);

        string path = Application.persistentDataPath + "/Capture" + DateTime.Now.ToString().Replace(':', '-') + ".png";
        ScreenCapture.CaptureScreenshot(path);
        LeanTween.delayedCall(0.2f, () =>
        {
            gameObject.SetActive(true); 
            g.SetActive(true);

            ImgurManager.instance.UploadImage(path,transform.position);
        });

    }

    public override void TPSResponse(int response)
    {
        flash.SetActive(true);
        LeanTween.delayedCall(0.1f, () => flash.SetActive(false));
    }


}
