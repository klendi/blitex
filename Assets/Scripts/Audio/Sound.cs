﻿/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .75f;

    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;

    [Range(0, 256)]
    public int priority = 128;

    [HideInInspector]
    public AudioSource audioSource;

}
