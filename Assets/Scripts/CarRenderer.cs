using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRenderer : MonoBehaviour
{
    public Transform[] wheels;
    public Transform[] parentWheels;

    public Entity entity;

    private float[] rots;

    private void Start()
    {
        //entity = GetComponent<Entity>();

        rots = new float[wheels.Length];

        for (int i = 0; i < wheels.Length; i++)
            rots[i] = 0f;
    }

    public void Update()
    {
        var data = ((float[],float[],sbyte[]))entity.additionalDataObject; //short for rpms, float for pos:y, byte for turn angles.

        for(int i = 0; i< wheels.Length; i++)
        {
            parentWheels[i].localRotation = Quaternion.AngleAxis(data.Item3[i] - 90, Vector3.up);

            wheels[i].localRotation = Quaternion.AngleAxis(rots[i], Vector3.forward);
            wheels[i].localPosition = Vector3.down * data.Item2[i];
            rots[i] += data.Item1[i] * Time.deltaTime;

            if (rots[i] >= 360000)
                rots[i] = 0;
        }
    }
}
