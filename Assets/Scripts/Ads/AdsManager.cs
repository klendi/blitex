using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    private void Awake()
    {
        string bannerADID = "ca-app-pub-2457877020060990/1885151063";
        BannerView bannerAd = new BannerView(bannerADID, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerAd.LoadAd(request);
        bannerAd.Show();
    }

    #region SHOP_ADS

    /* funcs for shop */
    /*
    public void ShowShopAdVideo()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleShopAdResult });
        }
    }

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
                SaveManager.Instance.data.diamonds += 2;
                SaveManager.Instance.Save();
                FindObjectOfType<NewShopManager>().UpdateText();
                print("Added 2 diamonds to player");
                break;

            default:
                break;
        }
    }*/

    /* Adds funcs for shop*/
    #endregion
}
