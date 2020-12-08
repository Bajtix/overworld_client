using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool updatePos = true;
    public int smoothingSpeed =  30;

    [System.NonSerialized]
    public int id;
    [System.NonSerialized]
    public string modelId;
    [System.NonSerialized]
    public int parentId;
    [System.NonSerialized]
    public string additionalData;
    public object additionalDataObject;

    private Quaternion destRot;
    private Vector3 destPos;

    /// <summary>
    /// Initializes entity with data
    /// </summary>
    /// <param name="id">The entities id, should be the same as server's</param>
    /// <param name="modelId">The spawned entity. </param>
    /// <param name="parentId">Parent entity</param>
    public void Initialize(int id,string modelId,int parentId)
    {
        this.id = id;
        this.modelId = modelId;
        this.parentId = parentId;
    }
    /// <summary>
    /// Sets the target position of the entity.
    /// </summary>
    /// <param name="position">The position data of the entity</param>
    /// <param name="rotation">The rotation data of the entity</param>
    /// <param name="additionalData">The additional data of the entity</param>
    public void SetTargets(Vector3 position, Quaternion rotation, string additionalData, object additionalDataObject)
    {
        destPos = position;
        destRot = rotation;
        this.additionalData = additionalData;
        this.additionalDataObject = additionalDataObject;
    }

    private void Update()
    {
        /// Smoothly transitions entity to target position and rotation
        
        if (updatePos)
        {
            transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * smoothingSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, destRot, Time.deltaTime * smoothingSpeed);
        }
    }
}
