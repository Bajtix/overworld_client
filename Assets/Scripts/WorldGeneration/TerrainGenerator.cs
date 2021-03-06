﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;
    private int latestId = 0;

    public Material grassy;
    public AudioSource wind;
    public float avgHeight;
    public GameObject lowLodV;
    public Dictionary<int, ChunkObject> objects;

    public ChunkManager.V2Int chunkCoords;

    public float _mtnNoiseScale = 0.02f;
    public float _baseDetailScale = 0.03f;
    public float _mountainPlateScale = 0.005f;

    public float mtnNoiseScale = 0.02f;
    public float baseDetailScale = 0.03f;
    public float detailLayers = 0f;
    public float mountainPlateScale = 0.0005f;

    public float mountainMainScale = 5;

    private void Start()
    {
        objects = new Dictionary<int, ChunkObject>();
        Generate();
    }
    /// <summary>
    /// Starts the Generation Chain
    /// </summary>
    public void Generate()
    {
        StartCoroutine("GenerateCoroutine");
    }
    /// <summary>
    /// Generates base terrain using perlin noise
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateCoroutine()
    {
        SetVars();

        //Debug.Log("GENERATE: SET VERTS");
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        List<float> heights = new List<float>();
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            var v = verts[i];
            verts[i].y = GetHeight(transform.position.x + v.x + TerrainSettings.instance.seed / 1.1233464534f,
                transform.position.z + v.z + TerrainSettings.instance.seed / 2 / 1.165882234141f);
            if (i % 400 == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            heights.Add(verts[i].y);
        }

        mesh.vertices = verts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
        avgHeight = 0;
        foreach (float f in heights)
        {
            avgHeight += f;
        }

        avgHeight /= heights.Count - 1;
        wind.transform.position = new Vector3(transform.position.x + 20, avgHeight, transform.position.z + 20);

        StartCoroutine("SpawnTrees");
        StartCoroutine("SpawnDetails");
        StartCoroutine("ApplyChunkMods");
        if (QualitySettings.GetQualityLevel() > 2)
        {
            StartCoroutine("SpawnGrass"); //only spawn grass on high and above
        }
    }
    /// <summary>
    /// Spawns trees and forests
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnTrees()
    {
        //Debug.Log("GENERATE: SPAWN TREES");


        System.Random r = new System.Random(Mathf.RoundToInt(TerrainSettings.instance.seed + transform.position.x + transform.position.z));

        float densityForChunk = Mathf.Pow(Mathf.PerlinNoise(transform.position.x * 0.00523f, transform.position.z * 0.00523f), 1.69f) * 25;

        for (int i = 0; i < densityForChunk; i++)
        {
            float x = r.Next(0, 40);
            float y = r.Next(0, 40);

            int selected = r.Next(0, TerrainSettings.instance.treePrefabs.Length);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x + x, 800, transform.position.z + y), new Vector3(0, -1, 0), out hit))
            {
                GameObject o = Instantiate(TerrainSettings.instance.treePrefabs[selected], hit.point, Quaternion.identity, transform);
                o.transform.Rotate(new Vector3(0, r.Next(0, 360), 0));
                AddFeature(o);
            }

            yield return new WaitForEndOfFrame();
        }
        //Debug.Log($"GENERATE: DONE (ChunkCount: {ChunkManager.instance.chunkCount})");
    }
    /// <summary>
    /// Spawns other features
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnDetails()
    {
        //Debug.Log("GENERATE: SPAWN DETAILS");


        System.Random r = new System.Random(Mathf.RoundToInt(TerrainSettings.instance.seed + transform.position.x + transform.position.z) / 3);

        float densityForChunk = r.Next(0, 3);

        for (int i = 0; i < densityForChunk; i++)
        {
            float x = r.Next(0, 40);
            float y = r.Next(0, 40);

            int selected = r.Next(0, TerrainSettings.instance.detailPrefabs.Length);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x + x, 1000, transform.position.z + y), new Vector3(0, -1, 0), out hit))
            {
                GameObject o = Instantiate(TerrainSettings.instance.detailPrefabs[selected], hit.point, Quaternion.identity, transform);
                o.transform.Rotate(new Vector3(0, r.Next(0, 360), 0));
                AddFeature(o);
            }

            yield return new WaitForEndOfFrame();
        }
        //Debug.Log($"GENERATE: DONE (ChunkCount: {ChunkManager.instance.chunkCount})");

        //grassObj = Instantiate(gameObject, transform, true);
        //grassObj.GetComponent<MeshRenderer>().material = Instantiate(grassy);

        //StartCoroutine("AssignTexture");
    }
    /// <summary>
    /// Spawns a copy of terrain with fewer polygons and applies a grass material
    /// </summary>
    /// <returns></returns>
    public IEnumerator SpawnGrass()
    {
        //Debug.Log("GENERATE: SET G VERTS");
        mesh = lowLodV.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;

        for (int i = 0; i < verts.Length; i++)
        {
            var v = verts[i];
            verts[i].y = GetHeight(transform.position.x + v.x + TerrainSettings.instance.seed / 1.1233464534f,
                transform.position.z + v.z + TerrainSettings.instance.seed / 2 / 1.165882234141f);
            if (i % 400 == 0)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        mesh.vertices = verts;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    /// <summary>
    /// Will be used to apply texture to the terrain
    /// </summary>
    /// <returns></returns>
    public IEnumerator AssignTexture()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        string pixelLog = "";
        var s = Mathf.RoundToInt(Mathf.Sqrt(verts.Length));
        Texture2D normals_tx = new Texture2D(s, s);

        /*for (int i = 0; i < verts.Length; i++)
        {

            Vector3 norm = transform.TransformDirection(normals[i]);
            
            float clor = Mathf.Pow(norm.y, 80);
            normals_tx.SetPixel(Mathf.FloorToInt(verts[i].x*2), Mathf.FloorToInt(verts[i].z * 2), new Color(verts[i].y, verts[i].y, verts[i].y));

            pixelLog = pixelLog + $"\n Added pixel at {Mathf.FloorToInt(verts[i].x * 2)} :: { Mathf.FloorToInt(verts[i].z * 2)} of color {verts[i].y}";

        }
        */
        int i = 0;
        pixelLog = $"Length of the array: {verts.Length}";
        for (int x = 0; x < s; x++)
        {
            for (int y = 0; y < s; y++)
            {
                Vector3 norm = transform.TransformDirection(normals[i]);

                float clor = Mathf.Pow(norm.y, 80);
                normals_tx.SetPixel(x, y, new Color(clor, clor, clor));

                pixelLog = pixelLog + $"\n Added pixel at {x} :: {y}   with i = {i}  of color {clor}";
                i++;
            }
        }
        yield return new WaitForEndOfFrame();
        normals_tx.Apply();
        Debug.Log("tex applied " + Application.persistentDataPath + "/file.png");
        byte[] png = normals_tx.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/file.png", png);
        File.WriteAllText(Application.persistentDataPath + "/filelog.txt", pixelLog);

        GetComponent<MeshRenderer>().material.SetTexture("_SplatMap", normals_tx);
    }

    public IEnumerator ApplyChunkMods()
    {
        List<ChunkMod> mods = ChunkManager.bufferedChunkMods[chunkCoords.x, chunkCoords.y];
        foreach (ChunkMod mod in mods)
        {
            if (mod.type == ChunkMod.ChunkModType.Add)
            {
                GameObject newObject = Instantiate(GameManager.instance.terrainObjectPrefabs[mod.modelId], mod.chunk, Quaternion.identity, transform);
                AddFeature(newObject);
            }
            /// Destroys a GameObject found by id on a chunk obtained from the chunk array.
            else if (mod.type == ChunkMod.ChunkModType.Remove)
            {
                RemoveFeature(mod.objectId);
            }
            yield return null;
        }
    }

    private float GetHeight(float i, float j)
    {
        //Base mounatins
        float baseMountains = Mathf.PerlinNoise(i * mtnNoiseScale, j * mtnNoiseScale);

        float mountainPlate = Mathf.PerlinNoise(i * mountainPlateScale, j * mountainPlateScale);
        float mountainGroundPlate = Mathf.PerlinNoise(i * mountainPlateScale / 2f, j * mountainPlateScale / 2f);
        float bias = Mathf.PerlinNoise(i * mountainPlateScale / 4f, j * mountainPlateScale / 4f) * 0.2f;
        float smoothness = (Mathf.PerlinNoise(i * mountainPlateScale * 4f, j * mountainPlateScale * 4f) * 0.4f) + .6f;


        baseMountains *= mountainPlate * mountainMainScale;
        baseMountains += bias;

        float pw = Mathf.PerlinNoise(i * mountainPlateScale, j * mountainPlateScale);
        float pdetail = Mathf.Pow(baseMountains, pw * 4.25f);

        //Creates and layers detail
        float detail = 0;
        for (int k = 1; k < detailLayers + 1f; k++)
        {
            float d = Mathf.PerlinNoise(i * baseDetailScale * k, j * baseDetailScale * k);
            detail += d / k;
        }

        detail *= smoothness;

        float hght = pdetail + detail * (1f / 6f);


        return hght / 1.5f * TerrainSettings.instance.multiplier;
    }

    private void SetVars()
    {
        mtnNoiseScale = _mtnNoiseScale * TerrainSettings.instance.noiseScale;
        baseDetailScale = _baseDetailScale * TerrainSettings.instance.noiseScale;
        mountainPlateScale = _mountainPlateScale * TerrainSettings.instance.noiseScale;
    }

    /// <summary>
    /// Adds a feature to the feature dictionary;
    /// </summary>
    /// <param name="go">GameObject to treat as a feature to add</param>
    public void AddFeature(GameObject go)
    {
        ChunkObject feature = go.AddComponent<ChunkObject>();
        feature.myId = objects.Count;
        feature.chunk = this;

        objects.Add(latestId, feature);
        latestId++;
    }
    /// <summary>
    /// Removes feature from the feature dictionary.
    /// </summary>
    /// <param name="id">The id of feature to remove</param>
    public void RemoveFeature(int id)
    {
        Destroy(objects[id].gameObject);
        objects.Remove(id);
    }
}
