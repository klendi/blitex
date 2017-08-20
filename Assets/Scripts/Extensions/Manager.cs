using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; set; }
    public int sceneIndex = 0;
    public bool soundOn = false;
    public Sprite[] soundSprites;
    PlayServices playGames;
    public GameObject[] playerPrefabs;
    public static int Counter { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        playGames = FindObjectOfType<PlayServices>();
        playGames.SignIn();
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
