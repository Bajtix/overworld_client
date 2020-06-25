using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRenderer : MonoBehaviour
{
    //public WheelCollider wheel;
    public Entity carEntity;
    private float motorAngle = 0;
    private float steerAngle = 0;
    public bool turn = false;
    public bool invert = false;
    public bool invertrpm = false;
    public float radiusMultiplier = 1;


    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        string[] n = carEntity.additionalData.Split(':');
        steerAngle = float.Parse(n[0]);
        float motorAnglew = float.Parse(n[1]);
        if(!invertrpm)
            motorAngle += motorAnglew / 60 * 360 * Time.deltaTime / radiusMultiplier;
        else
            motorAngle -= motorAnglew / 60 * 360 * Time.deltaTime / radiusMultiplier;
        if (turn)
        {
            if (!invert)
                transform.localRotation = Quaternion.AngleAxis(steerAngle + 90, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);
            else
                transform.localRotation = Quaternion.AngleAxis(steerAngle + 270, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);           
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(90, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);
        }
        
    }
}
