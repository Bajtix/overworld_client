/*using Imgur.API.Authentication; 
using Imgur.API.Endpoints;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using UnityEngine;

public class ImgurManager : MonoBehaviour
{

    private ApiClient apiClient;

    private void Start()
    {
        apiClient = new ApiClient("89bdb09b8ea2675");

        var httpClient = new HttpClient();

        var filePath = Application.persistentDataPath + "/screentest.png";
        using var fileStream = File.OpenRead(filePath);

        var imageEndpoint = new ImageEndpoint(apiClient, httpClient);
        var imageUpload = imageEndpoint.UploadImageAsync(fileStream);

        Debug.Log(imageUpload.Result.Link);
    }
}*/
