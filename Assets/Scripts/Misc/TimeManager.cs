using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public Material clouds;
    public GameObject sun;
    public long time;

    private Vector2 scroll = Vector2.zero;
    private void Awake()
    {
        if (instance != this) Destroy(instance);
        instance = this;
    }

    public void SetWorldTime(long time, float cloudDensity)
    {
        clouds.SetFloat("_Cutoff", cloudDensity);
        this.time = time;
        GameManager.worldTime = time; // change this to self time later

        //sun.transform.rotation = Quaternion.Euler(15 * time - 90, 0, 0);

        //sun.GetComponent<Light>().intensity = Mathf.Abs(time - 12) /12 * 2.3f;
    }

    private void Update()
    {
        clouds.SetFloat("_ScrollX", scroll.x);
        clouds.SetFloat("_ScrollY", scroll.y);
        
        scroll += new Vector2(0.2f, 0.1f) * Time.deltaTime;
    }
}
