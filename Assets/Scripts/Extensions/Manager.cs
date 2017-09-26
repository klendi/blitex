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
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }

    public int sceneIndex = 0;
    public bool soundOn = false;
    //TODO: This is version with ads disabled
    [SerializeField]
    bool ads_enabed = false;
    public bool adsEnabled
    {
        get
        {
            return ads_enabed;
        }
    }

    [Header("Resources")]
    public Sprite[] soundSprites;
    public GameObject[] playerPrefabs;
    public GameObject hitVFX;
    public GameObject diamondHitFx;
    public GameObject redDiamondHitFx;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public void OnSoundClick(Button btn, Button btn2)
    {
        soundOn = !soundOn;
        AudioListener.volume = 0;

        if (!soundOn)
        {
            btn.GetComponent<Image>().sprite = soundSprites[0];
            btn2.GetComponent<Image>().sprite = soundSprites[0];
            AudioListener.volume = 1;
        }
        if (soundOn)
        {
            btn.GetComponent<Image>().sprite = soundSprites[1];
            btn2.GetComponent<Image>().sprite = soundSprites[1];
            AudioListener.volume = 0;
        }
    }
    public void OnSoundClick(Button btn)
    {
        soundOn = !soundOn;
        AudioListener.volume = 0;

        if (!soundOn)
        {
            btn.GetComponent<Image>().sprite = soundSprites[0];
            AudioListener.volume = 1;
        }
        if (soundOn)
        {
            btn.GetComponent<Image>().sprite = soundSprites[1];
            AudioListener.volume = 0;
        }
    }
}
