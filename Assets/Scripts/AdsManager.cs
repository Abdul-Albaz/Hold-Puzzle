using System;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    private EventManager events => EventManager.Instance;
    public RewardedAds rewarded;
    public InterstitialAds interstitial;

    public static float timeLimit = 60;

    public bool shouldShowAds
    {
        get => PlayerPrefs.GetInt("shouldShowAds", 0) == 1;
        set => PlayerPrefs.SetInt("shouldShowAds", value ? 1 : 0);
    }

    public void ShowInter() { interstitial.Show(AdType.levelCompleted); }
    public void ShowRewarded() { rewarded.Show(AdType.levelFailed); }

    void Start()
    {
        #if UNITY_IPHONE
            //AMR.AMRSDK.startTestSuite(new string[] { AdIds.iOS.rewarded, AdIds.iOS.interstitial, AdIds.iOS.bannerHorizontal });
        #endif
        #if UNITY_ANDROID
            AMR.AMRSDK.startTestSuite(new string[] { AdIds.Android.rewarded, AdIds.Android.interstitial, AdIds.Android.banner });
        #endif

        rewarded = new RewardedAds();
        interstitial = new InterstitialAds();

        AMR.AMRSdkConfig config = new AMR.AMRSdkConfig();

        config.ApplicationIdAndroid = AdIds.Android.app;
        config.InterstitialIdAndroid = AdIds.Android.interstitial;
        config.RewardedVideoIdAndroid = AdIds.Android.rewarded;
        config.BannerIdAndroid = AdIds.Android.banner;

        //config.ApplicationIdIOS = AdIds.iOS.app;
        //config.InterstitialIdIOS= AdIds.iOS.interstitial;
        //config.RewardedVideoIdIOS= AdIds.iOS.rewarded;
        //config.BannerIdIOS = AdIds.iOS.banner;

        //Privacy COMPLIANCE
        config.UserConsent = "1";
        config.SubjectToGDPR = "1";
        config.SubjectToCCPA = "1";
        config.IsUserChild = "0";

        AMR.AMRSDK.startWithConfig(config, OnSDKDidInitialize);
    }

    private void OnSDKDidInitialize(bool success, String error)
    {
        if (!success) adPrint(error);
        else adPrint("init successful");

        // Banner Callbacks
        AMR.AMRSDK.setOnBannerReady(OnBannerReady);
        AMR.AMRSDK.setOnBannerFail(OnBannerFail);
        AMR.AMRSDK.setOnBannerClick(OnBannerClick);

        if (interstitial == null) interstitial = new();
        interstitial.init();

        if (rewarded == null) rewarded = new();
        rewarded.init();

        LoadBanner();
    }

    public void OnBannerFail(string error) { adPrint($"banner fail {error}"); }
    public void OnBannerClick(string networkName) { }

    public void OnBannerReady(string networkName, double ecpm) {
        events.log(EventActions.adReceived, AdFormat.banner, networkName, "USD", ecpm / 1000);
        events.log(EventActions.adDisplayed, AdFormat.banner, networkName, "USD", ecpm / 1000);
        events.log(EventActions.adBannerDisplayed, AdFormat.banner, networkName, "USD", ecpm / 1000);
    }

    public void LoadBanner() { AMR.AMRSDK.loadBanner(AMR.Enums.AMRSDKBannerPosition.BannerPositionBottom, true); }

    public static void adPrint(string message) { print("onat patates " + message); }

    static class AdIds
    {
        public static class Android
        {
            public static string app = "642f472b-5a73-40f4-8c50-810f249fe01d";
            public static string rewarded = "b3fb5642-6e83-4b05-9942-e57621643ab1";
            public static string interstitial = "5d791991-330d-482f-8b28-54994227e52c";
            public static string banner = "2062727a-b287-4a10-9e31-582bffd278be";
        }

        //public static class iOS
        //{
        //    public static string app = "test";
        //    public static string rewarded = "test";
        //    public static string interstitial = "test";
        //    public static string banner = "test";
        //}
    }

    public void Success(AdType adType) { }

    public void Fail(AdType adType) { events.log(EventActions.adFailedToShow, adType); }
}

public enum AdState
{
    success,
    fail
}

public enum AdFormat
{
    banner,
    interstitial,
    rewarded
}

public enum AdType
{
    levelFailed,
    levelCompleted
}