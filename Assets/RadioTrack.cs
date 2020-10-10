using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Track",menuName = "Game/Radio/Track")]
public class RadioTrack : ScriptableObject
{
    public string trackName;
    public string trackArtist;
    public AudioClip audioClip;
    
}
