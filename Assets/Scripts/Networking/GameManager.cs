using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary; //TY <3


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

    public static List<ChunkMod> bufferedChunkMods = new List<ChunkMod>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    [System.Serializable]
    public class _EDIC : SerializableDictionaryBase<string, GameObject> { }
    public _EDIC entityPrefabs;
    public GameObject[] terrainObjectPrefabs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnNewEntity(int id, Vector3 position, Quaternion rotation, string modelId, int parentId)
    {
        Entity _entity;
        if (parentId == -9999)
            _entity = Instantiate(entityPrefabs[modelId], position, rotation).GetComponent<Entity>();
        else
            _entity = Instantiate(entityPrefabs[modelId], position, rotation, GameManager.entities[parentId].transform).GetComponent<Entity>();

        _entity.Initialize(id,modelId,parentId);
        _entity.SetTargets(position, Quaternion.identity,"");
        entities.Add(id, _entity);
    }

    public void KillEntity(int id)
    {
        Destroy(entities[id].gameObject);
        entities.Remove(id);
    }

    public void ModChunk(ChunkMod c)
    {
        Debug.Log("Mod chunk received.");
        if (c.type == ChunkMod.ChunkModType.Add)
        {
            ChunkManager.V2Int v = ChunkManager.ChunkAt(c.chunk.x, c.chunk.z);
            GameObject w = ChunkManager.instance.chunks[v.x, v.y];
            GameObject g = Instantiate(terrainObjectPrefabs[c.modelId],c.chunk,Quaternion.identity,w.transform);
            w.GetComponent<TerrainGenerator>().AddFeature(g);
        }
        else
        {
            ChunkManager.V2Int v = ChunkManager.ChunkAt(c.chunk.x, c.chunk.z);
            GameObject w = ChunkManager.instance.chunks[v.x, v.y];
            w.GetComponent<TerrainGenerator>().RemoveFeature(c.objectId);
        }
    }
    
}
