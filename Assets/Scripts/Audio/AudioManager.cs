/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.loop = s.loop;
            s.audioSource.priority = s.priority;
        }
    }

    public IEnumerator FadeOut(string sound, float FadeTime)
    {

        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }

        float startVolume = s.volume;

        while (s.audioSource.volume > 0)
        {
            s.audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.audioSource.Stop();
    }

    public IEnumerator FadeIn(string sound, float FadeTime)
    {

        Sound s = Array.Find(sounds, item => item.name == sound);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }

        s.audioSource.pitch = s.pitch;

        float startVolume = 0.2f;

        s.audioSource.volume = 0;
        s.audioSource.Play();

        while (s.audioSource.volume < s.volume)
        {
            s.audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.audioSource.volume = s.volume;
    }

    public void PlaySound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.audioSource.volume <= s.volume)
        {
            s.audioSource.volume = 1;
        }

        s.audioSource.volume = s.volume;
        s.audioSource.pitch = s.pitch;

        s.audioSource.Play();
    }
    public void Pause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.audioSource.Stop();
    }
    public bool IsPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }

        return s.audioSource.isPlaying;
    }

}
