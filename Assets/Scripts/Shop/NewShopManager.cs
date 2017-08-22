//using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class NewShopManager : MonoBehaviour
{
    public Transform ballsPanel;
    public GameObject notEnoughMoneyTab, loadingTab;
    public Outline buyButtonColor;
    public Text buttonText, diamondsText, costText, NotEnoughDiamondsText;
    public VerticalScrollSnap scroll;

    //const string interstitialAdId = "ca-app-pub-2457877020060990/9029321528";
    //InterstitialAd interstitalAd = new InterstitialAd(interstitialAdId);

    private int[] ballCosts = { 0, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 300, 310, 320, 330, 340, 350 };
    public int activeBallIndex = 0;
    private int currentPage = 0;
    public int selectedBallIndex = 0;

    private void Awake()
    {
        if (SaveManager.Instance.data.activeBall == 0)
        {
            SaveManager.Instance.UnlockBall(0);     //if this is the first time we run the game and we want to have bought the default ball, and set it as bought
            SetBall(0);
        }
        //ShowInterstitalAd();

        scroll.StartingScreen = SaveManager.Instance.data.activeBall;
        notEnoughMoneyTab.SetActive(false);
        activeBallIndex = SaveManager.Instance.data.activeBall;
        SaveManager.Instance.data.diamonds += 50;
        OnNewPage();
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Main Menu");

        if (Time.timeSinceLevelLoad >= 30f)
        {
            //ShowInterstitalAd();
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
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }
    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:

                loadingTab.SetActive(false);
                notEnoughMoneyTab.SetActive(false);
                break;
            case ShowResult.Skipped:

                loadingTab.SetActive(false);
                notEnoughMoneyTab.SetActive(false);
                break;
            case ShowResult.Finished:

                loadingTab.SetActive(false);
                notEnoughMoneyTab.SetActive(false);
                break;

            default:
                break;
        }
    }
    
    /* End ads stuff */

    /* Interstital AD stuff*/
    //public void ShowInterstitalAd()
    //{
    //    //interstitalAd.OnAdLoaded += InterstitalAd_OnAdLoaded;
    //}
    //private void InterstitalAd_OnAdLoaded(object sender, System.EventArgs e)
    //{
    //    AdRequest request = new AdRequest.Builder().Build();
    //    //interstitalAd.LoadAd(request);
    //    print("Yay now the ad is loaded display it");
    //}
    /* End Interstital AD stuff*/

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
                SetBall(selectedBallIndex);
                UpdateText();
            }
            else
            {
                NotEnoughDiamondsText.text = "You need " + (ballCosts[selectedBallIndex] - SaveManager.Instance.data.diamonds);
                notEnoughMoneyTab.SetActive(true);
            }
        }
        OnNewPage();
    }
}
