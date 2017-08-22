using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;
    public GameObject googlePlaygameTab;
    public GameObject googlePlaygameTabExit;
    public GameObject gameModeTab;
    public GameObject playButtonOriginal;
    public Animator playButtonAnimator;
    public Button soundButton;
    public CanvasGroup cg;
    RectTransform playButtonTransform;
    bool hasPlayed = false;
    bool isAtGameMode = false, isAtInfoTab = false, isAtGameServices = false;

    private void Start()
    {
        cg.alpha = 0;
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);
        gameModeTab.SetActive(false);
        playButtonTransform = playButtonOriginal.GetComponent<RectTransform>();
        playButtonOriginal.GetComponent<Button>().onClick.AddListener(() => OnAnimStart());

        if (!Manager.Instance.soundOn)
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
        else
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
    }

    private void Update()
    {
        if (isAtGameMode && Input.GetKeyDown(KeyCode.Escape))
        {
            gameModeTab.SetActive(false);
            playButtonOriginal.GetComponent<RectTransform>().anchoredPosition3D = playButtonTransform.anchoredPosition3D;
            playButtonOriginal.GetComponent<RectTransform>().localScale = playButtonTransform.localScale;
        }
        else if (isAtInfoTab && Input.GetKeyDown(KeyCode.Escape))
        {
            OnInfoExitClicked();
        }
        else if (isAtGameServices && Input.GetKeyDown(KeyCode.Escape))
        {
            OnGameServiceExit();
        }
    }

    public void OnShopClicked()
    {
        SceneManager.LoadScene("Shop");
    }
    public void OnAnimStart()
    {
        if (!hasPlayed)
        {
            isAtGameMode = true;
            playButtonAnimator.SetBool("hasPressed", true);
            StartCoroutine(WaitForAnimToEndThenPlay());
            hasPlayed = true;
        }
        else if (hasPlayed)
        {
            isAtGameMode = false;
            SceneManager.LoadScene("LevelSelector");
        }
    }
    public void OnPlayClick()
    {
        isAtGameMode = false;
        SceneManager.LoadScene("LevelSelector");
    }
    public void OnFrozenPlay()
    {
        isAtGameMode = false;
        SceneManager.LoadScene("LevelSelectorSnow");
    }
    public void OnTwitterClick()
    {
        float startTime;
        startTime = Time.timeSinceLevelLoad;

        //open the twitter app
        Application.OpenURL("twitter:///user?screen_name=klendigocci");

        if (Time.timeSinceLevelLoad - startTime <= 3f)
        {
            //fail. Open chrome.
            Application.OpenURL("http://www.twitter.com/klendigocci");
        }
    }
    public void OnGameReset()
    {
        SaveManager.Instance.ResetSave();
        SaveManager.Instance.Load();
        print("Reset succesfully");
    }
    public void OnFacebookClick()
    {
        float startTime;
        startTime = Time.timeSinceLevelLoad;

        //open the fb app
        Application.OpenURL("fb:///page/1984682608469987");

        if (Time.timeSinceLevelLoad - startTime <= 3f)
        {
            //fail. Open chrome.
            Application.OpenURL("http://www.facebook.com/sublexgames");
        }
    }
    public void OnSoundClicked()
    {
        Manager.Instance.OnSoundClick(soundButton);
    }
    public void OnRatingClicked()
    {
        print("Now it loads the game");
    }

    public void OnInfoClicked()
    {
        cg.alpha = 0;
        infoLabel.SetActive(true);
        isAtInfoTab = true;
    }
    public void OnInfoExitClicked()
    {
        StartCoroutine(ExitThenWait(.65f, infoLabel, infoLabelExit));
        isAtInfoTab = false;
    }

    public void OnGameServiceClicked()
    {
        googlePlaygameTab.GetComponent<CanvasGroup>().alpha = 0;
        isAtGameServices = true;
        googlePlaygameTab.SetActive(true);
    }
    public void OnGameServiceExit()
    {
        isAtGameServices = false;
        StartCoroutine(ExitThenWait(.65f, googlePlaygameTab, googlePlaygameTabExit));
    }

    public void OnSignInClick()
    {
        Social.localUser.Authenticate(succes =>
        {
            if(succes)
            {
                Debug.Log("Succes loggin the player");
            }
            else if(!succes)
            {
                Debug.Log("Error loggin the player");
            }
        });
    }
    public void OnAchievementsClick()
    {
        PlayServices.ShowAchievementsUI();
    }
    public void OnLeaderBoardClick()
    {
        PlayServices.ShowLeaderBoardUI();
    }

    private IEnumerator WaitForAnimToEndThenPlay()
    {
        yield return new WaitForSeconds(.1f);
        gameModeTab.SetActive(true);
    }
    private IEnumerator ExitThenWait(float seconds, GameObject current, GameObject next)
    {
        current.SetActive(false);
        next.SetActive(true);
        yield return new WaitForSeconds(seconds);
        next.SetActive(false);
    }
}