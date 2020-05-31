using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRenderer : MonoBehaviour
{
    public WheelCollider wheel;
    
    private float motorAngle = 0;
    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        motorAngle += wheel.rpm / 60 * 360 * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(wheel.steerAngle + 90, Vector3.up) * Quaternion.AngleAxis(motorAngle, Vector3.forward);
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
