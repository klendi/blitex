/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)         //if already is a game object then destroy this and keep that
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        AudioManager.instance.PlaySound("Theme");
    }
}
