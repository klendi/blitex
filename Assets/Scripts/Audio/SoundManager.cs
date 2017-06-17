using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    [Range(0f,1f)]
    public float volume = 1f;
    public bool loop = false;
    public AudioClip[] sounds;
    public AudioSource audioSource;

    public void PlayClip(AudioClip clip)
    {
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayClip(AudioClip clip, Transform point)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        AudioSource.PlayClipAtPoint(clip, point.position);
    }
}
