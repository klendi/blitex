using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }
    public int sceneIndex = 0;
    public bool soundOn = false;
    public Sprite[] soundSprites;
    public GameObject[] playerPrefabs;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdsManager.Instance.showBannerAd();
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
