using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMod
{
    public enum ChunkModType
    {
        Add = 0,
        Remove = 1
    }
    public ChunkModType type;
    public Vector3 chunk;
    public int objectId;
    public int modelId;

    public ChunkMod(ChunkModType type, Vector3 chunk, int objectId, int modelId)
    {
        this.type = type;
        this.chunk = chunk;
        this.objectId = objectId;
        this.modelId = modelId;
    }
}
