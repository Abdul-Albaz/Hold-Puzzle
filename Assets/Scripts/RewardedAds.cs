using System.Threading.Tasks;
using UnityEngine;

public class RewardedAds
{
    private AdsManager manager => AdsManager.Instance;
    private EventManager events => EventManager.Instance;
    private AdType adType;

    private bool isCompleted = false;

    private bool isReadyToLoad() => AMR.AMRRewardedVideoView.Instance.state != AMR.AMRRewardedVideoView.VideoState.Loading && AMR.AMRRewardedVideoView.Instance.state != AMR.AMRRewardedVideoView.VideoState.Loaded;
    private bool isReadyToShow() => AMR.AMRSDK.isRewardedVideoReadyToShow();
    public float lastShowTime() => PlayerPrefs.GetFloat("adShowTime", 0);
    private void SetShowTime() { PlayerPrefs.SetFloat("adShowTime", Time.time); }

    public void init()
    {
        AdsManager.adPrint("rewarded init");
        SetShowTime();

        AMR.AMRSDK.setOnRewardedVideoReady(OnReady);
        AMR.AMRSDK.setOnRewardedVideoFail(OnFail);
        AMR.AMRSDK.setOnRewardedVideoFailToShow(OnFailToShow);
        AMR.AMRSDK.setOnRewardedVideoShow(OnShow);
        AMR.AMRSDK.setOnRewardedVideoClick(OnClick);
        AMR.AMRSDK.setOnRewardedVideoDismiss(OnDismiss);
        AMR.AMRSDK.setOnRewardedVideoComplete(OnComplete);
        AMR.AMRSDK.setOnRewardedVideoImpression(OnImpression);
        AMR.AMRSDK.setOnRewardedVideoStatusChange(OnStatusChange);
        TryToLoad();
    }

    public void OnReady(string networkName, double ecpm) { events.log(EventActions.adReceived, AdFormat.rewarded, networkName, value: ecpm / 1000); }
    public void OnFail(string errorMessage) { events.log(EventActions.adFailed, AdFormat.rewarded, errorMessage); }
    public void OnFailToShow() { }
    public void OnClick(string networkName) { }
    public void OnComplete() { isCompleted = true; } //Grant reward
    public void OnStatusChange(int status) { }

    public void Success() => manager.Success(adType);
    public void Fail() => manager.Fail(adType);

    public void TryToLoad()
    {
        Load();
        Task.Delay(1000).ContinueWith(t => Load());
    }

    void Load()
    {
        try { if (isReadyToLoad()) AMR.AMRSDK.loadRewardedVideo(); }
        catch { }
    }

    public void Show(AdType adType)
    {
        if (!manager.shouldShowAds) return;
        this.adType = adType;

        if (AMR.AMRSDK.isRewardedVideoReady()) AMR.AMRSDK.showRewardedVideo();
        else
        {
            TryToLoad();
            manager.interstitial.Show((AdType)System.Enum.Parse(typeof(AdType), System.Enum.GetName(typeof(AdType), adType)));
        }
    }


    public void OnShow()
    {
        isCompleted = false;
        TryToLoad();
        SetShowTime();
    }

    public void OnDismiss()
    {
        TryToLoad();
        if (isCompleted) Success();
        else Fail(); 
    }

    public void OnImpression(AMR.AMRAd ad)
    {
        events.log(EventActions.adDisplayed, AdFormat.rewarded, ad.Network, ad.Currency, ad.Revenue);
        events.log(EventActions.adRewardedDisplayed, AdFormat.rewarded, ad.Network, ad.Currency, ad.Revenue);
    }
}
