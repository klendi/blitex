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
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    const string MenuTheme = "MenuTheme";
    const string LevelTheme = "LevelTheme";

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

    /// <summary>Crossfades to the menu music (used by the menu screens).</summary>
    public void PlayMenuMusic()
    {
        CrossFade(LevelTheme, MenuTheme);
    }

    /// <summary>Crossfades to the level music (used while playing a level).</summary>
    public void PlayLevelMusic()
    {
        CrossFade(MenuTheme, LevelTheme);
    }

    void CrossFade(string outgoing, string incoming)
    {
        if (IsPlaying(outgoing))
            StartCoroutine(FadeOut(outgoing, .25f));

        if (!IsPlaying(incoming))
            StartCoroutine(FadeIn(incoming, .5f));
    }

    public IEnumerator FadeOut(string sound, float fadeTime)
    {
        Sound s = Find(sound);
        if (s == null)
            yield break;

        float startVolume = s.volume;

        while (s.audioSource.volume > 0)
        {
            s.audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        s.audioSource.Stop();
    }

    public IEnumerator FadeIn(string sound, float fadeTime)
    {
        Sound s = Find(sound);
        if (s == null)
            yield break;

        s.audioSource.pitch = s.pitch;

        float step = 0.2f;

        s.audioSource.volume = 0;
        s.audioSource.Play();

        while (s.audioSource.volume < s.volume)
        {
            s.audioSource.volume += step * Time.deltaTime / fadeTime;

            yield return null;
        }

        s.audioSource.volume = s.volume;
    }

    public void PlaySound(string sound)
    {
        Sound s = Find(sound);
        if (s == null)
            return;

        s.audioSource.volume = s.volume;
        s.audioSource.pitch = s.pitch;

        s.audioSource.Play();
    }

    public void Pause(string sound)
    {
        Sound s = Find(sound);
        if (s == null)
            return;

        s.audioSource.Stop();
    }

    public bool IsPlaying(string sound)
    {
        Sound s = Find(sound);
        return s != null && s.audioSource.isPlaying;
    }

    Sound Find(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
            Debug.LogWarning("Sound: " + sound + " not found!");

        return s;
    }
}
