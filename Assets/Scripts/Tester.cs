using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public RadioStation r;
    public long time;
    public string trackName;
    public float trackTime;
    private float w = 0;

    public AudioSource source;
    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
            gameObject.SetActive(false);

        /*RadioStation.TrackData data = r.GetTrackAtTime(time);

        trackName = data.clip.trackName;
        trackTime = data.clipTime;
        w += Time.deltaTime;
        if (w >= 1)
        {
            time += 1;
            w = 0;
            source.Stop();
            source.time = data.clipTime;
            source.clip = data.clip.audioClip;
            source.Play();
        }*/
        
    }
}
