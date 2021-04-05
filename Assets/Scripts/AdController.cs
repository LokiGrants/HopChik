using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "4076646";
#elif UNITY_ANDROID
    private string gameId = "4076647";
    #endif

    bool testMode = true;

    public string placementIdBanner = "";
    public string placementIdInterstitial = "video";
    public string placementIdRewardVideo = "rewardedVideo";

    public GameObject adsThanksPanel;
    public GameObject adsDidntWorkPanel;
    public GameObject adsSkippedPanel;
    public GameObject adsNotReadyPanel;

    // Start is called before the first frame update
    void Start()
    {
        // this is the listener for the ad services
        Advertisement.AddListener(this);

        Advertisement.Initialize(gameId, testMode);

        // Run the banner as a coroutine after initialization is done for adverts
        StartCoroutine(ShowBannerWhenInitialized());
    }


    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }

        //Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        // Load banner using my placementID
        //Advertisement.Banner.Show(placementIdBanner);
    }


    public void ShowInterstitialAd()
    {
        // check if UnityAds is ready before calling the show method
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementIdInterstitial);
        }
        else
        {
            adsNotReadyPanel.SetActive(true);
        }
    }


    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady(placementIdRewardVideo))
        {
            Advertisement.Show(placementIdRewardVideo);
        }
        else
        {
            adsNotReadyPanel.SetActive(true);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Special code to stop your game or music
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            // method here to allow the user to revive
            adsThanksPanel.SetActive(true);
        }
        else if (showResult == ShowResult.Skipped)
        {
            adsSkippedPanel.SetActive(true);
        }
        else if (showResult == ShowResult.Failed)
        {
            adsDidntWorkPanel.SetActive(true);
        }
    }

    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
