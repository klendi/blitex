﻿/*
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
/// I know the name of class doesnt connect with it, i was drunk when wrote it
/// </summary>
public class LevelShopManager : MonoBehaviour
{
    public GameObject infoLabel;
    public GameObject infoLabelExit;
    public GameObject googlePlaygameTab;
    public GameObject googlePlaygameTabExit;
    public GameObject gameModeTab;
    public GameObject playButtonOriginal;
    public Animator playButtonAnimator;
    public Button soundButton, soundButton2;
    public CanvasGroup cg;
    bool hasPlayed = false;
    bool shownInterstital = false;
    bool isAtInfoTab = false, isAtGameServices = false;

    private void Start()
    {
        FindObjectOfType<AdsManager>().ShowInterstitalAd();
        cg.alpha = 0;
        infoLabel.SetActive(false);
        infoLabelExit.SetActive(false);
        gameModeTab.SetActive(false);
        playButtonOriginal.GetComponent<Button>().onClick.AddListener(() => OnAnimStart());
        FindObjectOfType<AudioManager>().Pause("LevelTheme");

        if (!FindObjectOfType<AudioManager>().IsPlaying("MenuTheme"))
        {
            FindObjectOfType<AudioManager>().PlaySound("MenuTheme");
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

        if (!AdsManager.Instance.interstitalLoaded && !shownInterstital && Random.Range(0, 100) <= 40)
        {
            AdsManager.Instance.ShowInterstitalAd();
        }
        else if (AdsManager.Instance.interstitalLoaded)
        {
            shownInterstital = true;
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
            playButtonAnimator.SetBool("hasPressed", true);
            StartCoroutine(WaitForAnimToEndThenPlay());
            hasPlayed = true;
        }
        else if (hasPlayed)
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
        Manager.Instance.OnSoundClick(soundButton, soundButton2);
    }
    public void OnRatingClicked()
    {
        AdsManager.Instance.ShowInterstitalAd();
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