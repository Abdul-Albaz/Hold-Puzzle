using System.Threading.Tasks;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class InterstitialAds
{
    private AdsManager manager => AdsManager.Instance;
    private EventManager events => EventManager.Instance;
    private AdType adType;

    private bool isLoading = false;
    //private bool isReadyToLoad() => AMR.AMRInterstitialView.Instance.state != AMR.AMRInterstitialView.InterstitialState.Loading && AMR.AMRInterstitialView.Instance.state != AMR.AMRInterstitialView.InterstitialState.Loaded; 
    private bool isReadyToShow() => ad.IsLoaded();
    private bool isOverTimeLimit() => Time.time - lastShowTime() > AdsManager.timeLimit;
    public float lastShowTime() => PlayerPrefs.GetFloat("adShowTime");
    private void SetShowTime() { PlayerPrefs.SetFloat("adShowTime", Time.time); }

    private InterstitialAd ad;

    public void init()
    {
        AdsManager.adPrint("interstitial init");
        SetShowTime();

#if UNITY_ANDROID
        string adUnitId = AdsManager.AdIds.Android.interstitial;
#elif UNITY_IPHONE
                string adUnitId = AdsManager.AdIds.iOS.interstitial;
#endif

        ad = new InterstitialAd(adUnitId);
        ad.OnAdLoaded += OnReady;
        ad.OnAdFailedToLoad += OnFail;
        ad.OnAdOpening += OnShow;
        ad.OnAdClosed += OnDismiss;
    }

    public void OnReady(object sender, EventArgs args)
    {
        isLoading = false;
        AdsManager.adPrint("interstitial ready");
        //events.log(EventActions.adReceived, AdFormat.interstitial, networkName, value: ecpm / 1000);
    }

    public void OnFail(object sender, AdFailedToLoadEventArgs args)
    {
        isLoading = false;
        events.log(EventActions.adFailed, AdFormat.interstitial, args.LoadAdError.GetMessage());
        events.log(EventActions.adInterstitialFailed, AdFormat.interstitial, args.LoadAdError.GetMessage());
    }

    public void OnShow(object sender, EventArgs args) {
        SetShowTime();
        //events.log(EventActions.adDisplayed, AdFormat.interstitial, ad.Network, ad.Currency, ad.Revenue);
        //events.log(EventActions.adInterstitialDisplayed, AdFormat.interstitial, ad.Network, ad.Currency, ad.Revenue);
    }

    public void OnDismiss(object sender, EventArgs args)
    {
        TryToLoad();
        Success();
    }

    public void Success() => manager.Success(adType);
    public void Fail() => manager.Fail(adType);

    public void TryToLoad()
    {
        Load(0);
        UnityMainThreadDispatcher.Instance().Enqueue(Load(1));
    }

    private IEnumerator Load(float delay)
    {
        yield return new WaitForSeconds(delay);
        try
        {
            if (!ad.IsLoaded())
            {
                AdRequest request = new AdRequest.Builder().Build();
                ad.LoadAd(request);
            }
        }
        catch { }
    }

    public void Show(AdType adType)
    {
        AdsManager.adPrint($"isOverTimeLimit: {isOverTimeLimit()} - Time: {Time.time} - lastShowTime: {lastShowTime()} - shouldShowAds: {manager.shouldShowAds}");
        if (!manager.shouldShowAds) return;
        this.adType = adType;

        if (!isOverTimeLimit()) { return; }

        events.log(EventActions.adRequestedToDisplay);

        if (isReadyToShow()) ad.Show();
        else
        {
            TryToLoad();
            Fail();
        }
    }
}
