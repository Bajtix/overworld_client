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

    

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    [System.Serializable]
    public class _EDIC : SerializableDictionaryBase<string, GameObject> { }
    public _EDIC entityPrefabs;
    public GameObject[] terrainObjectPrefabs;
    public List<Item> items;

    public static long worldTime = 0;

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

    /// <summary>
    /// Gets an Item by name from the GameManager's item registry.
    /// </summary>
    /// <param name="name">The name of the item</param>
    /// <returns>Returns the found Item if it exists, othervise returns null.</returns>
    public Item GetItem(string name)
    {
        foreach (Item i in items)
            if (i.name == name)
                return i;
        return null;
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
    /// <summary>
    /// Spawns new entity into the world.
    /// </summary>
    /// <param name="id">New entity's ID</param>
    /// <param name="position">New entity's position</param>
    /// <param name="rotation">New entity's rotation</param>
    /// <param name="modelId"> The entity to spawn </param>
    /// <param name="parentId">New entity's parent id (-9999 if none) </param>
    public void SpawnNewEntity(int id, Vector3 position, Quaternion rotation, string modelId, int parentId)
    {
        Entity _entity;
        if (parentId == -9999)
            _entity = Instantiate(entityPrefabs[modelId], position, rotation).GetComponent<Entity>();
        else
            _entity = Instantiate(entityPrefabs[modelId], position, rotation, GameManager.entities[parentId].transform).GetComponent<Entity>();

        _entity.Initialize(id,modelId,parentId);
        _entity.SetTargets(position, Quaternion.identity,"",null);
        entities.Add(id, _entity);
    }

    /// <summary>
    /// Kills an entity.
    /// </summary>
    /// <param name="id">Id of the entity to kill</param>
    public void KillEntity(int id)
    {
        Destroy(entities[id].gameObject);
        entities.Remove(id);
    }

    /// <summary>
    /// Mods a chunk. 
    /// </summary>
    /// <param name="mod">Chunk to mod</param>
    public void ModChunk(ChunkMod mod)
    {
        ChunkManager.V2Int chunkPosition = ChunkManager.ChunkAt(mod.chunk.x, mod.chunk.z);
        GameObject targetChunk = ChunkManager.instance.chunks[chunkPosition.x, chunkPosition.y];
        ChunkManager.bufferedChunkMods[chunkPosition.x, chunkPosition.y].Add(mod);
        /// Creates a new GameObject and parents it to a corresponding chunk, obtained by checking the chunk array.
        if (mod.type == ChunkMod.ChunkModType.Add)
        {            
            GameObject newObject = Instantiate(terrainObjectPrefabs[mod.modelId],mod.chunk,Quaternion.identity,targetChunk.transform);
            targetChunk.GetComponent<TerrainGenerator>().AddFeature(newObject);
        }
        /// Destroys a GameObject found by id on a chunk obtained from the chunk array.
        else if (mod.type == ChunkMod.ChunkModType.Remove)
        {           
            targetChunk.GetComponent<TerrainGenerator>().RemoveFeature(mod.objectId);
        }
    }
    
}
