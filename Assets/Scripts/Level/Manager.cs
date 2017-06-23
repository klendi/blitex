using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }
    public int sceneIndex = 1;
    public bool soundOn = false;
    public Sprite[] soundSprites;
    public GameObject[] playerPrefabs;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnSoundClick(Button btn)
    {
        soundOn = !soundOn;
        AudioListener.pause = soundOn;

        if (soundOn)
        {
            btn.GetComponent<Image>().sprite = soundSprites[0];
        }
        if (!soundOn)
        {
            btn.GetComponent<Image>().sprite = soundSprites[1];
        }
    }
}
