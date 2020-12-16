using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class Picture : MonoBehaviour
{

    Entity e;

    bool downloaded = false;

    private void Start()
    {
        e = GetComponent<Entity>();
    }

    private void Update()
    {
        if(e.additionalDataObject != null && !downloaded)
        {
            string url = (string)e.additionalDataObject;
            Debug.Log(url);
            downloaded = true;

            StartCoroutine("DownloadAndLoad",url);
        }
    }

    IEnumerator DownloadAndLoad(string url)
    {
        Texture2D tex;
        tex = new Texture2D(1920, 1080);
        using (WWW www = new WWW(url))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
    }


}
