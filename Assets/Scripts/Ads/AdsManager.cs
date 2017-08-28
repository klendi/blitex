/*
================================================================
    Product:    Blitex
    Developer:  Klendi Gocci - klendigocci@gmail.com
    Date:       26/8/2017. 11:58
================================================================
   Copyright (c) Klendi Gocci.  All rights reserved.
================================================================
*/

using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; set; }
    public bool interstitalLoaded = false;
    public bool interstitalClosed = false;
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
    }
}
