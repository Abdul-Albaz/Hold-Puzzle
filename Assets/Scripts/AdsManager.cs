using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAdsMediationTestSuite.Api;
using GoogleMobileAds.Api.Mediation.IronSource;
using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Api.Mediation.Fyber;

public class AdsManager : Singleton<AdsManager>
{
    private EventManager events => EventManager.Instance;
    public RewardedAds rewarded;
    public InterstitialAds interstitial;
    public BannerView banner;

    public static float timeLimit = 1;

    public bool shouldShowAds
    {
        get => PlayerPrefs.GetInt("shouldShowAds", 1) == 1;
        set => PlayerPrefs.SetInt("shouldShowAds", value ? 1 : 0);
    }

    public void ShowInter() { interstitial.Show(AdType.levelCompleted); }
    public void ShowRewarded() { rewarded.Show(AdType.levelFailed); }

    void Start()
    {
        IronSource.SetConsent(true);
        UnityAds.SetConsentMetaData("gdpr.consent", true);
        Fyber.SetGDPRConsent(true);
        //Fyber.SetGDPRConsentString("myGDPRConsentString");

        ShowMediationTestSuite();
        
        rewarded = new RewardedAds();
        interstitial = new InterstitialAds();

        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder().SetSameAppKeyEnabled(true).build();

        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize(OnSDKInit);
    }

    private void OnSDKInit(InitializationStatus initStatus)
    {
        Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
        foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
        {
            string className = keyValuePair.Key;
            AdapterStatus status = keyValuePair.Value;
            switch (status.InitializationState)
            {
                case AdapterState.NotReady:
                    // The adapter initialization did not complete.
                    adPrint("Adapter: " + className + " not ready.");
                    break;
                case AdapterState.Ready:
                    // The adapter was successfully initialized.
                    adPrint("Adapter: " + className + " is initialized.");
                    break;
            }
        }
        
        interstitial.init();
        rewarded.init();

        LoadBanner();
        
        MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;

        banner.OnAdLoaded += OnBannerReady;
        banner.OnAdFailedToLoad += OnBannerFail;
        banner.OnAdOpening += OnBannerClick;
        banner.OnAdClosed += OnBannerDismiss;
    }

    public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
    }


    public void OnBannerReady(object sender, EventArgs args) {
        //events.log(EventActions.adReceived, AdFormat.banner, networkName, "USD", ecpm / 1000);
        //events.log(EventActions.adDisplayed, AdFormat.banner, networkName, "USD", ecpm / 1000);
        //events.log(EventActions.adBannerDisplayed, AdFormat.banner, networkName, "USD", ecpm / 1000);
    }

    public void OnBannerFail(object sender, AdFailedToLoadEventArgs args)
    {
        events.log(EventActions.adFailed, AdFormat.banner, args.LoadAdError.GetMessage());
        events.log(EventActions.adBannerFailed, AdFormat.banner, args.LoadAdError.GetMessage());
    }

    public void OnBannerClick(object sender, EventArgs args) { }

    public void OnBannerDismiss(object sender, EventArgs args) { LoadBanner(); }


    public void LoadBanner()
    {
        #if UNITY_ANDROID
                string adUnitId = AdIds.Android.banner;
        #elif UNITY_IPHONE
                string adUnitId = AdIds.iOS.banner;
        #endif
        banner = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);
    }

    private void ShowMediationTestSuite()
    {
        MediationTestSuite.Show();
    }

    public static class AdIds
    {
        public static class Android
        {
            public static string app = "ca-app-pub-1638526051519794~9183469575";
            public static string rewarded = "ca-app-pub-1638526051519794/1802156000";
            public static string interstitial = "ca-app-pub-1638526051519794/9608493833";
            public static string banner = "ca-app-pub-1638526051519794/6220408004";

            //public static string rewarded = "ca-app-pub-3940256099942544/5224354917";
            //public static string interstitial = "ca-app-pub-3940256099942544/1033173712";
            //public static string banner = "ca-app-pub-3940256099942544/6300978111";
        }

        public static class iOS
        {
            public static string app = "71a09b62-befd-48a0-a3be-edfa2b1b1b70";
            public static string rewarded = "ca-app-pub-3940256099942544/1712485313";
            public static string interstitial = "ca-app-pub-3940256099942544/4411468910";
            public static string banner = "ca-app-pub-3940256099942544/2934735716";
        }
    }

    public void Success(AdType adType) { }
    public void Fail(AdType adType) { events.log(EventActions.adFailedToShow, adType); }

    public static void adPrint(string message) { Debug.Log("onat patates " + message); }
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