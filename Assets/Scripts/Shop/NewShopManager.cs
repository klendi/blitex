/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       23/8/2017. 14:29
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

/// <summary>
/// The class that handle shop things
/// </summary>
public class NewShopManager : MonoBehaviour
{
    public Transform ballsPanel;
    public GameObject notEnoughMoneyTab, loadingTab;
    public Outline buyButtonColor;
    public Text buttonText, diamondsText, costText, NotEnoughDiamondsText;
    public VerticalScrollSnap scroll;
    bool loadedVideo = false;
    bool showedInterstitalAd = false;
    bool thisTimeShowInterstital = false;

    public int activeBallIndex = 0;
    private int currentPage = 0;
    public int selectedBallIndex = 0;
    public int[] ballCosts = { 0, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 300, 310, 320, 330, 340, 350 };

    private void Awake()
    {
        if (SaveManager.Instance.data.activeBall == 0)
        {
            SaveManager.Instance.UnlockBall(0);     //if this is the first time we run the game and we want to have bought the default ball, and set it as bought
            SetBall(0);
        }

        scroll.StartingScreen = SaveManager.Instance.data.activeBall;
        notEnoughMoneyTab.SetActive(false);
        activeBallIndex = SaveManager.Instance.data.activeBall;
        OnNewPage();
        UpdateText();

        if (Random.Range(0, 100) <= 45)
        {
            //now this time we gonna show interstital ad
            print("This time gonna show interstital, $$");
            thisTimeShowInterstital = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");

        if (!AdsManager.Instance.interstitalLoaded && !showedInterstitalAd && thisTimeShowInterstital)
        {
            AdsManager.Instance.ShowInterstitalAd();
        }
        else if (AdsManager.Instance.interstitalLoaded)
        {
            showedInterstitalAd = true;
            thisTimeShowInterstital = false;
        }

    }

    public void OnNewPage()
    {
        currentPage = scroll.CurrentPage;
        OnBallSelect(currentPage);
    }

    public void CloseNotEnoughTab()
    {
        notEnoughMoneyTab.SetActive(false);
    }

    /* Video Ads stuff */
    public void ShowAdVideo()
    {
        loadingTab.SetActive(true);
        loadingTab.GetComponentInChildren<Text>().text = "Loading Video";
        if (Advertisement.IsReady())
        {
            loadedVideo = true;
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }
    private void HandleAdResult(ShowResult result)
    {
        notEnoughMoneyTab.SetActive(false);

        switch (result)
        {
            case ShowResult.Failed:
                print("Failed, maybe internet");
                loadingTab.GetComponentInChildren<Text>().text = "Failed To Load Video";
                StartCoroutine(WaitForCertainTimeThenfalse());
                break;
            case ShowResult.Skipped:
                print("Skiped the add");
                loadingTab.GetComponentInChildren<Text>().text = "Video Skipped";
                notEnoughMoneyTab.SetActive(false);
                StartCoroutine(WaitThenSetFalse());
                loadedVideo = true;
                break;
            case ShowResult.Finished:
                print("GAVE THE MONEY TO THE PLAYER");
                loadingTab.GetComponentInChildren<Text>().text = "Succes";
                SaveManager.Instance.data.diamonds += 5;
                StartCoroutine(WaitThenSetFalse());
                loadedVideo = true;
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitThenSetFalse()
    {
        yield return new WaitForSeconds(1f);
        loadingTab.SetActive(false);
    }
    private IEnumerator WaitForCertainTimeThenfalse()
    {
        //if video its not ready for 4 sec then fail it
        yield return new WaitForSeconds(4f);
        if (!loadedVideo)
        {
            loadingTab.GetComponentInChildren<Text>().text = "Failed To Load Video";
            yield return new WaitForSeconds(1.5f);
            loadingTab.SetActive(true);
        }
    }
    /* End Video ads stuff */

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnBallSelect(int index)
    {
        if (selectedBallIndex == index)
            return;

        selectedBallIndex = index;

        if (SaveManager.Instance.DoesOwnBall(selectedBallIndex))
        {
            if (activeBallIndex == selectedBallIndex)
            {
                buttonText.text = "Current";
                buyButtonColor.effectColor = new Color(0f, 234f / 255f, 1f);
            }
            else
            {
                buttonText.text = "Select";
                buyButtonColor.effectColor = new Color(0f, 1f, 171f / 255f);
            }
        }
        else
        {
            if (SaveManager.Instance.data.diamonds >= ballCosts[selectedBallIndex])
            {
                //he can afford it but he hasnt buyed yet, set the color to green
                buttonText.text = "Buy";
                buyButtonColor.effectColor = new Color(0f, 1f, 171f / 255f);
            }
            else if (SaveManager.Instance.data.diamonds < ballCosts[selectedBallIndex])
            {
                //he cant afford it
                buttonText.text = "Buy";
                buyButtonColor.effectColor = new Color(1f, 0f, 90f / 255f);
            }
        }

        costText.text = string.Format("Price: {0}", ballCosts[index].ToString());
    }

    public void UpdateText()
    {
        diamondsText.text = SaveManager.Instance.data.diamonds.ToString();
    }

    private void SetBall(int index)
    {
        activeBallIndex = index;
        SaveManager.Instance.data.activeBall = activeBallIndex;
        buttonText.text = "Current";
        buyButtonColor.effectColor = new Color(0f, 234f / 255f, 1f);
    }

    public void OnBallBuy()
    {
        if (SaveManager.Instance.DoesOwnBall(selectedBallIndex))
        {
            SetBall(selectedBallIndex);
        }
        else
        {
            if (SaveManager.Instance.BuyBall(selectedBallIndex, ballCosts[selectedBallIndex]))
            {
                //succes he bough the ball
                PlayServices.UnlockAchievement(GPGSIds.achievement_first_purchase);
                PlayServices.IncrementAchievement(GPGSIds.achievement_5_balls_purchased, 1);
                SetBall(selectedBallIndex);
                UpdateText();
            }
            else
            {
                //he dont have enough diamonds to buy it :C
                NotEnoughDiamondsText.text = "You need " + (ballCosts[selectedBallIndex] - SaveManager.Instance.data.diamonds);
                notEnoughMoneyTab.SetActive(true);
            }
        }
        OnNewPage();
    }
}
