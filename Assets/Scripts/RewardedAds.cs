using System.Threading.Tasks;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class RewardedAds
{
    private AdsManager manager => AdsManager.Instance;
    private EventManager events => EventManager.Instance;
    private AdType adType;

    private bool isCompleted = false;
    //private bool isReadyToLoad() => AMR.AMRInterstitialView.Instance.state != AMR.AMRInterstitialView.InterstitialState.Loading && AMR.AMRInterstitialView.Instance.state != AMR.AMRInterstitialView.InterstitialState.Loaded; 
    private bool isReadyToShow() => ad.IsLoaded();
    private bool isOverTimeLimit() => Time.time - lastShowTime() > AdsManager.timeLimit;
    public float lastShowTime() => PlayerPrefs.GetFloat("adShowTime");
    private void SetShowTime() { PlayerPrefs.SetFloat("adShowTime", Time.time); }

    private RewardedAd ad;

    public void init()
    {
        AdsManager.adPrint("rewarded init");
        SetShowTime();
#if UNITY_ANDROID
        string adUnitId = AdsManager.AdIds.Android.rewarded;
#elif UNITY_IPHONE
        string adUnitId = AdsManager.AdIds.iOS.rewarded;
#endif

        ad = new RewardedAd(adUnitId);
        ad.OnAdLoaded += OnReady;
        ad.OnAdFailedToLoad += OnFail;
        ad.OnAdOpening += OnShow;
        ad.OnAdClosed += OnDismiss;
        ad.OnAdFailedToShow += OnFailToShow;
        ad.OnUserEarnedReward += OnComplete;

        TryToLoad();
    }

    public void OnReady(object sender, EventArgs args)
    {
        AdsManager.adPrint("rewarded ready");
        //events.log(EventActions.adReceived, AdFormat.rewarded, networkName, value: ecpm / 1000);
    }

    public void OnFail(object sender, AdFailedToLoadEventArgs args)
    {
        events.log(EventActions.adFailed, AdFormat.rewarded, args.LoadAdError.GetMessage());
        events.log(EventActions.adInterstitialFailed, AdFormat.rewarded, args.LoadAdError.GetMessage());
    }

    public void OnShow(object sender, EventArgs args)
    {
        SetShowTime();
        isCompleted = false;
        //events.log(EventActions.adDisplayed, AdFormat.rewarded, ad.Network, ad.Currency, ad.Revenue);
        //events.log(EventActions.adInterstitialDisplayed, AdFormat.rewarded, ad.Network, ad.Currency, ad.Revenue);
    }

    public void OnFailToShow(object sender, AdErrorEventArgs args)
    {
        events.log(EventActions.adFailed, AdFormat.rewarded, args.AdError.GetMessage());
        events.log(EventActions.adInterstitialFailed, AdFormat.rewarded, args.AdError.GetMessage());
    }

    public void OnComplete(object sender, Reward args)
    {
        isCompleted = true;
    }


    public void OnDismiss(object sender, EventArgs args)
    {
        TryToLoad();
        if (isCompleted) Success();
        else Fail();
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
        try {
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
        if (!manager.shouldShowAds) return;
        this.adType = adType;

        events.log(EventActions.adRequestedToDisplay);

        if (isReadyToShow()) ad.Show();
        else
        {
            TryToLoad();
            manager.interstitial.Show(adType);
        }
    }
}
