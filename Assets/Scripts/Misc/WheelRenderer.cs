using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRenderer : MonoBehaviour
{
    public WheelCollider wheel;
    public Entity carEntity;
    private float motorAngle = 0;
    private float steerAngle = 0;
    public bool turn = false;
    public bool invert = false;

    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        steerAngle = float.Parse(carEntity.additionalData);
        //motorAngle += wheel.rpm / 60 * 360 * Time.deltaTime;
        if (turn)
        {
            if (!invert)
                transform.localRotation = Quaternion.AngleAxis(steerAngle + 90, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);
            else
                transform.localRotation = Quaternion.AngleAxis(steerAngle + 270, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);           
        }
        var wheelCCenter = wheel.transform.TransformPoint(wheel.center);
        RaycastHit hit;
        if (Physics.Raycast(wheelCCenter, -Vector3.up, out hit, wheel.suspensionDistance + wheel.radius))
        {
            transform.position = hit.point + (Vector3.up * wheel.radius);
        }
        else
        {
            transform.position = wheelCCenter - (Vector3.up * wheel.suspensionDistance);
        }
    }
}
