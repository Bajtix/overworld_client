using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public int mass;
    public AudioClip[] audioClips;
    public AudioSource source;

    private void Start()
    {
        if(source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.clip = audioClips[Random.Range(0,audioClips.Length)];
        source.pitch = (1 / mass); 
        source.volume = collision.relativeVelocity.magnitude;
        source.Play();
    }
}
