using System.Threading.Tasks;
using UnityEngine;

public class InterstitialAds
{
    private AdsManager manager => AdsManager.Instance;
    private EventManager events => EventManager.Instance;
    private AdType adType;

    private bool isReadyToLoad() => AMR.AMRInterstitialView.Instance.state != AMR.AMRInterstitialView.InterstitialState.Loading && AMR.AMRInterstitialView.Instance.state != AMR.AMRInterstitialView.InterstitialState.Loaded; 
    private bool isReadyToShow() => AMR.AMRSDK.isInterstitialReadyToShow();
    private bool isOverTimeLimit() => Time.time - lastShowTime() > AdsManager.timeLimit;
    public float lastShowTime() => PlayerPrefs.GetFloat("adShowTime");
    private void SetShowTime() { PlayerPrefs.SetFloat("adShowTime", Time.time); }

    public void init()
    {
        AdsManager.adPrint("interstitial init");
        SetShowTime();

        AMR.AMRSDK.setOnInterstitialReady(OnReady);
        AMR.AMRSDK.setOnInterstitialFail(OnFail);
        AMR.AMRSDK.setOnInterstitialFailToShow(OnFailToShow);
        AMR.AMRSDK.setOnInterstitialShow(OnShow);
        AMR.AMRSDK.setOnInterstitialClick(OnClick);
        AMR.AMRSDK.setOnInterstitialDismiss(OnDismiss);
        AMR.AMRSDK.setOnInterstitialImpression(OnImpression);
        AMR.AMRSDK.setOnInterstitialStatusChange(OnStatusChange);
        TryToLoad();
    }

    public void OnReady(string networkName, double ecpm) { events.log(EventActions.adReceived, AdFormat.interstitial, networkName, value: ecpm / 1000); }
    public void OnFail(string errorMessage) { events.log(EventActions.adFailed, AdFormat.interstitial, errorMessage); }
    public void OnFailToShow() { }
    public void OnShow() { SetShowTime(); }
    public void OnClick(string networkName) { }
    public void OnStatusChange(int status) { }

    public void Success() => manager.Success(adType);
    public void Fail() => manager.Fail(adType);

    public void TryToLoad()
    {
        Load();
        Task.Delay(1000).ContinueWith(t => Load());
    }

    private void Load()
    {
        try { if (isReadyToLoad()) AMR.AMRSDK.loadInterstitial(); }
        catch { }
    }

    public void Show(AdType adType)
    {
        AdsManager.adPrint($"isOverTimeLimit: {isOverTimeLimit()} - Time: {Time.time} - lastShowTime: {lastShowTime()}");
        if (!manager.shouldShowAds) return;
        this.adType = adType;

        if (!isOverTimeLimit()) { return; }

        events.log(EventActions.adRequestedToDisplay);

        if (AMR.AMRSDK.isInterstitialReady()) AMR.AMRSDK.showInterstitial();
        else
        {
            TryToLoad();
            Fail();
        }
    }

    public void OnDismiss()
    {
        TryToLoad();
        Success();
    }

    public void OnImpression(AMR.AMRAd ad)
    {
        events.log(EventActions.adDisplayed, AdFormat.interstitial, ad.Network, ad.Currency, ad.Revenue);
        events.log(EventActions.adInterstitialDisplayed, AdFormat.interstitial, ad.Network, ad.Currency, ad.Revenue);
    }
}
