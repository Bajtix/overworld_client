using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider[] motorWheels;
    public WheelCollider[] turningWheels;
    public WheelCollider[] oppositeTurningWheels;
    private Rigidbody rb;
    

    public float speed;
    public float brake = 1000;
    public float turnAngle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        foreach (WheelCollider wheel in motorWheels)
        {
            wheel.motorTorque = -speed * Input.GetAxis("Vertical");
            float Vel = -transform.InverseTransformDirection(rb.velocity).z;
            if ((Vel * Input.GetAxis("Vertical")) < 0f)
            {
                Debug.Log($"Brake. {Vel} * {Input.GetAxis("Vertical")}");
                wheel.brakeTorque = brake;
            }
            else
                wheel.brakeTorque = 0;
        }

        foreach (WheelCollider wheel in turningWheels)
        {
            wheel.steerAngle = turnAngle * Input.GetAxis("Horizontal");
        }

        foreach (WheelCollider wheel in oppositeTurningWheels)
        {
            wheel.steerAngle = turnAngle * -Input.GetAxis("Horizontal");
        }
    }
}
