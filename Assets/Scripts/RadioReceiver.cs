using UnityEngine;

public class RadioReceiver : MonoBehaviour
{
    public AudioSource source;
    public RadioStation station;
    private long localTime = -1;
    private void Update()
    {
        if (localTime != GameManager.worldTime)
        {
            RadioStation.TrackData data = station.GetTrackAtTime(GameManager.worldTime);
            source.Stop();
            source.time = data.clipTime;
            source.clip = data.clip.audioClip;
            source.Play();
            localTime = GameManager.worldTime;
        }
    }
}
