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

    public GameObject[] particles;
    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
    }
    private void Update()
    {
        string[] eData = carEntity.additionalData.Split(':');
        if (eData.Length != 3) return;
        
        steerAngle = float.Parse(eData[0]);
        float motorAnglew = float.Parse(eData[1]);
        bool playerIn = bool.Parse(eData[2]);
        if (playerIn)
        {
            foreach (GameObject o in particles)
            {
                o.SetActive(true);
            }

            if (!carEntity.GetComponent<AudioSource>().isPlaying)
                carEntity.GetComponent<AudioSource>().Play();
        }
        else
        {
            carEntity.GetComponent<AudioSource>().Stop();
            foreach (GameObject o in particles)
            {
                o.SetActive(false);
            }
        }

        if (!invertrpm)
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
