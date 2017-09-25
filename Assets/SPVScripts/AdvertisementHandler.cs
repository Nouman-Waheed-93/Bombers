using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;

public class AdvertisementHandler : MonoBehaviour {

    public static AdvertisementHandler instance;
    
    public delegate void RewardMethodDel();
    RewardMethodDel RewardNow;
    
    InterstitialAd interstitial;
   // RewardBasedVideoAd rewardedAd;

    const string RewardedPlacementId = "rewardedVideo";

    void Awake() {
        
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Advertisement.Initialize("1473588");
        
        }
        
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-5493138044756388/8844078759";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void ShowInterstitial() {

        if (interstitial.IsLoaded())
            interstitial.Show();

        Invoke("RequestInterstitial", 1);    

    }
    
    //Unity Ads Region
    #region Unity Ads
    public void ShowRewardedAd(RewardMethodDel RMD)
    {
        RewardNow = RMD;
        const string RewardedPlacementId = "rewardedVideo";
        print("Here we are");
        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);
        
    }
    
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                RewardNow();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }

    }

    #endregion
    
}
