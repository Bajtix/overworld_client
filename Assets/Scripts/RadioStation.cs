using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game/Radio/Station",fileName ="New Station")]
public class RadioStation : ScriptableObject
{
    public string stationName;
    public string stationOwner;
    public float alertFrequency;
    public RadioTrack[] radioTracks;

    private float radioLength = -1;
    
    public struct TrackData
    {
        public RadioTrack clip;
        public float clipTime;

        public TrackData(RadioTrack clip, float clipTime)
        {
            this.clip = clip;
            this.clipTime = clipTime;
        }
    }

    public void CalculateLength()
    {
        Debug.Log("Calculated length");
        radioLength = 0;
        foreach (RadioTrack t in radioTracks)
            radioLength += t.audioClip.length;

        Debug.Log(radioLength);
    }

    public TrackData GetTrackAtTime(long time)
    {

        if (radioLength <= 0)
            CalculateLength();

        float runningTime = time % Mathf.RoundToInt(radioLength);
        int index = 0;
        while (runningTime >= radioTracks[index].audioClip.length)
        {
            runningTime -= radioTracks[index].audioClip.length;
            index++;
        }
        return new TrackData(radioTracks[index],runningTime);
    }
}
