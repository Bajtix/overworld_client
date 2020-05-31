using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int id;
    public int modelId;

    private Quaternion destRot;
    private Vector3 destPos;

    public void Initialize(int id,int modelId)
    {
        this.id = id;
        this.modelId = modelId;
    }

    public void SetTargets(Vector3 p, Quaternion r)
    {
        destPos = p;
        destRot = r;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destPos, Time.deltaTime * 30);
        transform.rotation = Quaternion.Lerp(transform.rotation, destRot, Time.deltaTime * 30);
    }
}
