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
        Debug.Log("Running time: " + runningTime);
        //RadioTrack last = null;
        int index = 0;
        while (runningTime >= radioTracks[index].audioClip.length)
        {
            runningTime -= radioTracks[index].audioClip.length;
            Debug.Log("Skipping track: " + radioTracks[index].trackName + "; Running time:" + runningTime) ;
            index++;
        }
        Debug.Log("Finished with track " + radioTracks[index].trackName + "with index " + index + " and time " + runningTime);
        return new TrackData(radioTracks[index],runningTime);
    }
}
