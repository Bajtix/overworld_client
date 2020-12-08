using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [Serializable]
    public struct CarData
    {
        public float steerAngle;
        public float rpm;
        public bool steered;

        public CarData(float steerAngle, float rpm, bool steered)
        {
            this.steerAngle = steerAngle;
            this.rpm = rpm;
            this.steered = steered;
        }
    }

   
}
