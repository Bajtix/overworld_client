using System.IO;
using UnityEngine;


public class ImgurManager : MonoBehaviour
{

    private ImgurClient imgurClient ; //key goes here


    public static ImgurManager instance;

    private Vector3 pos;

    private void Awake()
    {
        imgurClient = new ImgurClient(File.ReadAllText(Application.dataPath + "imgurKey.txt"));


        instance = this;
    }


    private void OnEnable()
    {
        imgurClient.OnImageUploaded += ImgurClient_OnImageUploaded;
    }

    private void ImgurClient_OnImageUploaded(object sender, ImgurClient.OnImageUploadedEventArgs e)
    {
        Debug.Log("Upload Complete: " + e.response.data.link);

        ClientSend.RequestEntity("prp_pic", pos, Quaternion.identity, e.response.data.link);
    }


    public void UploadImage(string path,Vector3 camPos)
    {
        imgurClient.UploadImageFromFilePath(path);
        pos = camPos;
    }
}

