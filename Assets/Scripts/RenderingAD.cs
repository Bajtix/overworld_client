using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingAD : MonoBehaviour
{
    private void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float[32];
        distances[9] = 1500000;
        camera.layerCullDistances = distances;
    }
}
