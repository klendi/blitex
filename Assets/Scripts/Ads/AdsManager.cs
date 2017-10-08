/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       3/10/2017. 20:36
================================================================
    Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// The Class that handles ads
/// </summary>
public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; set; }
    [HideInInspector]
    public bool interstitalLoaded = false;
    [HideInInspector]
    public bool interstitalClosed = false;

    float timeCounter = 0f;
    [Tooltip("The interval of showing ads (in seconds)")]
    public float intervalOfShowingAds = 60;   //1 minutes = 60 sec
    [HideInInspector]
    public bool canShowAd = false;
    [HideInInspector]
    public bool needsToShowAd = false;

    const string interstitialAdId = "ca-app-pub-2457877020060990/9029321528";
    InterstitialAd interstitalAd;
    AdRequest request;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (Manager.Instance.adsEnabled)
        {
            interstitalAd = new InterstitialAd(interstitialAdId);

            //we show the banner ad at the start of main menu
            string bannerADID = "ca-app-pub-2457877020060990/1885151063";
            BannerView bannerAd = new BannerView(bannerADID, AdSize.SmartBanner, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            bannerAd.LoadAd(request);
            bannerAd.Show();

            request = new AdRequest.Builder().Build();
            interstitalAd.LoadAd(request);
        }
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;

        //user isnt playing a level, hes anywhere except playing so the time is up and ready to show a ad
        if (canShowAd && Mathf.Round(timeCounter) >= Mathf.Round(intervalOfShowingAds) && Manager.Instance.adsEnabled && !needsToShowAd)
        {
            ShowInterstitalAd();
            timeCounter = 0;
            print((Mathf.Round(timeCounter) + " seconds Passed, be ready to see a exciting ad rn"));
        }
        //the time's up but user is playing, waiting till he finish
        else if(!canShowAd && Mathf.Round(timeCounter) >= Mathf.Round(intervalOfShowingAds) && Manager.Instance.adsEnabled && !needsToShowAd)
        {
            needsToShowAd = true;
            print("The time's up but we will wait till u finish ur current level afterwards ur gonna se ad");
        }
        //the user finished his level so now we show ad
        if (needsToShowAd && canShowAd && Manager.Instance.adsEnabled)
        {
            print("Waited enough, Now its perfect to show a big ad");
            ShowInterstitalAd();
            needsToShowAd = false;
            //reseting so counting starts over again
            timeCounter = 0;
        }
    }

    /* <Interstital Ad>*/
    public void ShowInterstitalAd()
    {
        request = new AdRequest.Builder().Build();
        interstitalAd.LoadAd(request);

        if (interstitalAd.IsLoaded())
        {
            interstitalLoaded = true;
            interstitalAd.Show();
            print("Loaded and showed Interstital Ad");
        }
        else
        {
            print("Not ready yet");
        }

        interstitalAd.OnAdClosed += InterstitalAd_OnAdClosed;
    }
    private void InterstitalAd_OnAdClosed(object sender, System.EventArgs e)
    {
        print("Succes, the ad is closed now");
        interstitalLoaded = false;
        interstitalClosed = true;
        needsToShowAd = false;
    }
    /* </Interstital Ad>*/

    public void ShowVideoAd()
    {
        Advertisement.Show("rewardedVideo");
    }


}
