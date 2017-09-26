/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The class that handle main menu
/// </summary>
//i know that the name of the class if weird to its function but nevermind it does it job
public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;
    public GameObject googlePlaygameTab;
    public GameObject googlePlaygameTabExit;
    public GameObject gameModeTab;
    public GameObject playButtonOriginal;
    public GameObject rateUsTab;
    public GameObject noAdsExplanation;
    public Animator playButtonAnimator;
    public Button soundButton, soundButton2;   //this is to keep track of that sound button
    public CanvasGroup cg;

    bool hasPlayed = false;
    bool shownInterstital = false;
    bool thisTimeShowInterstital = false;
    bool isAtInfoTab = false, isAtGameServices = false;
    private bool showedInterstitalAd = false;

    private void Start()
    {

        cg.alpha = 0;
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);
        gameModeTab.SetActive(false);
        rateUsTab.SetActive(false);
        playButtonOriginal.GetComponent<Button>().onClick.AddListener(() => OnAnimStart());

        if (AdsManager.Instance.mainMenuAdsNum >= 3 && !AdsManager.Instance.interstitalLoaded && Manager.Instance.adsEnabled)
        {
            if (Random.Range(0, 6) == 1)
            {
                //Advertisement.Show("rewardedVideo");
                AdsManager.Instance.ShowVideoAd();
            }
            else
            {
                print("Time to show some interstital ad at succes");
                AdsManager.Instance.ShowInterstitalAd();
            }
            AdsManager.Instance.mainMenuAdsNum = 0;
        }
        else if (AdsManager.Instance.mainMenuAdsNum <= 3)
        {
            AdsManager.Instance.mainMenuAdsNum++;
        }

        if (FindObjectOfType<AudioManager>().IsPlaying("LevelTheme"))
        {
            StartCoroutine(FindObjectOfType<AudioManager>().FadeOut("LevelTheme", .25f));
        }

        if (!FindObjectOfType<AudioManager>().IsPlaying("MenuTheme"))
        {
            StartCoroutine(FindObjectOfType<AudioManager>().FadeIn("MenuTheme", .5f));
        }

        if ((SaveManager.Instance.data.completedLevels == 20 || SaveManager.Instance.data.completedSnowLevels == 14) && SaveManager.Instance.data.hasRatedGame)
        {
            rateUsTab.SetActive(true);
        }

        if (!Manager.Instance.soundOn)
        {
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
            soundButton2.GetComponent<Image>().sprite = Manager.Instance.soundSprites[0];
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
            soundButton2.GetComponent<Image>().sprite = Manager.Instance.soundSprites[1];
        }
    }

    private void Update()
    {
        if (isAtInfoTab && Input.GetKeyDown(KeyCode.Escape))
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
        if (!hasPlayed && SaveManager.Instance.data.hasUnlockedSnowTheme)
        {
            playButtonAnimator.SetBool("hasPressed", true);
            StartCoroutine(WaitForAnimToEndThenPlay());
            hasPlayed = true;
        }
        else if (hasPlayed || !SaveManager.Instance.data.hasUnlockedSnowTheme)
        {
            SceneManager.LoadScene("LevelSelector");
        }
    }
    public void OnPlayClick()
    {
        SceneManager.LoadScene("LevelSelector");
    }
    public void OnFrozenPlay()
    {
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
        Application.OpenURL("http://www.facebook.com/klendigocci");
    }
    public void OnSoundClicked()
    {
        Manager.Instance.OnSoundClick(soundButton, soundButton2);
    }
    public void OnRatingClicked()
    {
        SaveManager.Instance.Save();
        Application.OpenURL("market://details?id=com.klendigocci.blitexx");
    }
    public void OnRatingTabClicked()
    {
        SaveManager.Instance.data.hasRatedGame = true;
        SaveManager.Instance.Save();
        Application.OpenURL("market://details?id=com.klendigocci.blitexx");
    }
    public void OnRatingTabExit()
    {
        rateUsTab.SetActive(false);
    }
    public void OnRemoveAds()
    {
        if (Manager.Instance.adsEnabled)
            noAdsExplanation.SetActive(true);
    }
    public void GoToItchIo()
    {
        if (Manager.Instance.adsEnabled)
        {
            Application.OpenURL("https://klendigocci.itch.io/blitex-pro");
            noAdsExplanation.SetActive(false);
        }
    }
    public void CloseRemoveAds()
    {
        if (Manager.Instance.adsEnabled)
        {
            noAdsExplanation.SetActive(false);
        }
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
        noAdsExplanation.SetActive(false);
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
            if (succes)
            {
                Debug.Log("Succes loggin the player");
            }
            else if (!succes)
            {
                Debug.Log("Error loggin the player");
            }
        });
    }
    public void OnInterstitalAdShow()
    {
        AdsManager.Instance.ShowInterstitalAd();
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