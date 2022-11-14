using System;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    public bool shouldShowAds;

    void Start()
    {
#if UNITY_IPHONE
        //     AMR.AMRSDK.startTestSuite(new string[] {"IOS_ZONE_ID","ANOTHER_IOS_ZONE_ID"});
#endif
#if UNITY_ANDROID
            AMR.AMRSDK.startTestSuite(new string[] { "test", "test" });
#endif
        AMR.AMRSdkConfig config = new AMR.AMRSdkConfig();
        config.ApplicationIdAndroid = "test";
        config.BannerIdAndroid = "test";
        config.InterstitialIdAndroid = "test";
        config.RewardedVideoIdAndroid = "test";

        // config.ApplicationIdIOS = "<Your IOS App Id>";
        // config.BannerIdIOS = "<Your IOS Banner Zone Id>";
        // config.InterstitialIdIOS = "<Your IOS Interstitial Zone Id>";
        // config.RewardedVideoIdIOS = "<Your IOS Video Zone Id>";


        //Privacy COMPLIANCE
        config.UserConsent = "1";
        config.SubjectToGDPR = "1";
        config.SubjectToCCPA = "1";
        config.IsUserChild = "1";

        AMR.AMRSDK.startWithConfig(config, OnSDKDidInitialize);

        EventManager.Instance.logEvent(EventActions.deneme);
    }


    private void OnSDKDidInitialize(bool success, String error)
    {
        if (!success) adPrint(error);
        else adPrint("init successful");

        // Banner Callbacks
        AMR.AMRSDK.setOnBannerReady(OnBannerReady);
        AMR.AMRSDK.setOnBannerFail(OnBannerFail);
        AMR.AMRSDK.setOnBannerClick(OnBannerClick);

        // Interstitial Ad Callbacks
        AMR.AMRSDK.setOnInterstitialReady(OnInterstitialReady);
        AMR.AMRSDK.setOnInterstitialFail(OnInterstitialFail);
        AMR.AMRSDK.setOnInterstitialFailToShow(OnInterstitialFailToShow);
        AMR.AMRSDK.setOnInterstitialShow(OnInterstitialShow);
        AMR.AMRSDK.setOnInterstitialImpression(OnInterstitialImpression);
        AMR.AMRSDK.setOnInterstitialClick(OnInterstitialClick);
        AMR.AMRSDK.setOnInterstitialDismiss(OnInterstitialDismiss);
        AMR.AMRSDK.setOnInterstitialStatusChange(OnInterstitialStatusChange);

        // Rewarded Ad Callbacks
        AMR.AMRSDK.setOnRewardedVideoReady(OnRewardedReady);
        AMR.AMRSDK.setOnRewardedVideoFail(OnRewardedFail);
        AMR.AMRSDK.setOnRewardedVideoFailToShow(OnRewardedFailToShow);
        AMR.AMRSDK.setOnRewardedVideoShow(OnRewardedShow);
        AMR.AMRSDK.setOnRewardedVideoImpression(OnRewardedImpression);
        AMR.AMRSDK.setOnRewardedVideoClick(OnRewardedClick);
        AMR.AMRSDK.setOnRewardedVideoDismiss(OnRewardedDismiss);
        AMR.AMRSDK.setOnRewardedVideoComplete(OnRewardedComplete);
        AMR.AMRSDK.setOnRewardedVideoStatusChange(OnRewardedStatusChange);

        LoadBanner();

    }

    public static void adPrint(string message) { print("onat patates " + message); }


    //Banner Callbacks
    public void OnBannerReady(string networkName, double ecpm) {
        adPrint($"banner ready - network: {networkName} - cpm: " + (ecpm / 1000));
        EventManager.Instance.logEvent(EventActions.adDisplayed, AdFormat.banner, networkName, "USD", ecpm/1000);
        EventManager.Instance.logEvent(EventActions.adBannerDisplayed, AdFormat.banner, networkName, "USD", ecpm / 1000);
    }

    public void OnBannerFail(string error) { adPrint($"banner fail {error}"); }

    public void OnBannerClick(string networkName) { }


    //Interstitial Ad Callbacks
    public void OnInterstitialReady(string networkName, double ecpm) { adPrint($"inter ready - network: {networkName} - cpm: " + (ecpm / 1000)); }

    public void OnInterstitialFail(string error)
    {
        adPrint($"inter fail {error}");
        LoadInterstitial();
    }

    public void OnInterstitialFailToShow() { }

    public void OnInterstitialShow() { adPrint("inter displayed"); }

    public void OnInterstitialImpression(AMR.AMRAd ad) {
        EventManager.Instance.logEvent(EventActions.adDisplayed, AdFormat.interstitial, ad.Network, ad.Currency, ad.Revenue);
        EventManager.Instance.logEvent(EventActions.adInterstitialDisplayed, AdFormat.interstitial, ad.Network, ad.Currency, ad.Revenue);
    }

    public void OnInterstitialClick(string networkName) { }

    public void OnInterstitialDismiss() { AMR.AMRSDK.loadInterstitial(); }

    public void OnInterstitialStatusChange(int status) { }


    //Rewarded Ad Callbacks
    public void OnRewardedReady(string networkName, double ecpm) { adPrint($"rewarded ready - network: {networkName} - cpm: " + (ecpm / 1000)); }

    public void OnRewardedFail(string error)
    {
        LoadRewarded();
        adPrint($"rewarded fail {error}");
    }

    public void OnRewardedShow() { adPrint("rewarded displayed"); }

    public void OnRewardedFailToShow() { }

    public void OnRewardedImpression(AMR.AMRAd ad)
    {
        EventManager.Instance.logEvent(EventActions.adDisplayed, AdFormat.rewarded, ad.Network, ad.Currency, ad.Revenue);
        EventManager.Instance.logEvent(EventActions.adRewardedDisplayed, AdFormat.rewarded, ad.Network, ad.Currency, ad.Revenue);
    }

    public void OnRewardedClick(string networkName) { }

    public void OnRewardedDismiss()
    {
        AMR.AMRSDK.loadInterstitial();
    }

    public void OnRewardedComplete() { adPrint("reward granted"); }

    public void OnRewardedStatusChange(int status) { }


    //Utility Functions
    public void LoadBanner()
    {
        AMR.AMRSDK.loadBanner(AMR.Enums.AMRSDKBannerPosition.BannerPositionBottom, true);
    }

    public void LoadInterstitial()
    {
        adPrint("loading inter");
        AMR.AMRSDK.loadInterstitial();
    }

    public void LoadRewarded()
    {
        adPrint("loading rewarded");
        AMR.AMRSDK.loadRewardedVideo();
    }

    public void ShowInterstitial()
    {
        adPrint("show inter: " + shouldShowAds);

        if (AMR.AMRSDK.isInterstitialReady() & shouldShowAds)
            AMR.AMRSDK.showInterstitial();
    }

    public void ShowRewarded()
    {
        adPrint("show rewarded: " + shouldShowAds);

        if (AMR.AMRSDK.isRewardedVideoReady() & shouldShowAds)
            AMR.AMRSDK.showRewardedVideo();
    }
}

public enum AdFormat
{
    banner,
    interstitial,
    rewarded
}