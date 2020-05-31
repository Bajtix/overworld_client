using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSettings : MonoBehaviour
{
    public static TerrainSettings instance;

    public float seed;
    public float noiseScale = 0.0124f;
    public float multiplier = 10f;

    public GameObject[] treePrefabs;
    public GameObject[] detailPrefabs;
    public int renderDistance = 1;

    private void Awake()
    {
        if (instance != this)
            if (instance == null)
                instance = this;
            else
                Destroy(this);

        seed = Random.Range(0f, 9999f);
    }

}
