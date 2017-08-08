using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        if (FindObjectOfType<LevelManager>() != null)
        {
            showBannerAd();
        }
    }
    public void showBannerAd()
    {
        string adID = "ca-app-pub-3940256099942544/6300978111";

        //***For Testing in the Device***
        AdRequest request = new AdRequest.Builder()
       .AddTestDevice("dc0be7b182029808")  // My test device.
       .Build();

        //***For Production When Submit App***
        //AdRequest request = new AdRequest.Builder().Build();

        BannerView bannerAd = new BannerView(adID, AdSize.SmartBanner, AdPosition.BottomLeft);
        bannerAd.LoadAd(request);
    }

    public void ShowAdVideo()
    {
        if (Advertisement.IsReady())
        {
            //Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
            Advertisement.Show();

        }
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                print("AD FAILED TO PLAY, Maybe internet");
                break;
            case ShowResult.Skipped:
                print("Player skipped the add");
                break;
            case ShowResult.Finished:
                print("Has finished the ad");
                break;

            default:
                break;
        }
    }

    #region SHOP_ADS
    /* funcs for shop */
    public void ShowShopAdVideo()
    {
        if (Advertisement.IsReady())
        {
            //Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleShopAdResult });
            Advertisement.Show();

        }
    }

    public void OnAdFinishedShop()
    {
        SaveManager.Instance.data.diamonds += 2;
        SaveManager.Instance.Save();
        print("Added diamonds to player");
    }
    //callback for showAdVideo

    private void HandleShopAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                print("AD FAILED TO PLAY, Maybe internet");
                break;
            case ShowResult.Skipped:
                print("Player skipped the add");
                break;
            case ShowResult.Finished:
                OnAdFinishedShop();
                break;

            default:
                break;
        }
    }
    /* Adds funcs for shop*/
    #endregion
}
