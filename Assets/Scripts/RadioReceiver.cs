using UnityEngine;

public class RadioReceiver : MonoBehaviour
{
    public AudioSource source;
    public RadioStation station;
    private long localTime = -1;

    private void Start()
    {
        station.CalculateLength();
    }


    private void Update()
    {
        if (localTime != GameManager.worldTime)
        {
            RadioStation.TrackData data = station.GetTrackAtTime(GameManager.worldTime);
            Debug.LogWarning($"Radio Debug: Time: {GameManager.worldTime} , {data.clip.trackName}");
            source.Stop();
            source.time = data.clipTime;
            source.clip = data.clip.audioClip;
            source.Play();
            localTime = GameManager.worldTime;
        }
    }
}
