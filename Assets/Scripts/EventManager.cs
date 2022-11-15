using System;
using Facebook.Unity;
using Firebase.Analytics;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
    Dictionary<string, object> metaBundle = new();
    Parameter[] firebaseBundle;

    public void Test() { log(EventActions.test); }

    public async void log(EventActions action)
    {
        FirebaseAnalytics.LogEvent(action.ToString());
        FB.LogAppEvent(action.ToString());

        AdsManager.adPrint($"{action}");
    }

    public async void log(EventActions action, AdType adType)
    {
        Bundle(action, adType);

        FB.LogAppEvent(action.ToString(), parameters: metaBundle);
        FirebaseAnalytics.LogEvent(action.ToString(), firebaseBundle);
    }

    public async void log(EventActions action, AdFormat adFormat, string error)
    {
        Bundle(action, adFormat, error);

        FirebaseAnalytics.LogEvent(action.ToString(), firebaseBundle);
        FB.LogAppEvent(action.ToString(), parameters: metaBundle);
    }

    public async void log(EventActions action, AdFormat adFormat, string platform = "", string currency = "USD", double value = 0)
    {
        Bundle(action, adFormat, platform, currency, value);

        FirebaseAnalytics.LogEvent(action.ToString(), firebaseBundle);
        FB.LogAppEvent(action.ToString(), parameters: metaBundle);
    }

    private void Bundle(EventActions action, AdType adType)
    {
        metaBundle.Clear();
        metaBundle.Add("adType", adType.ToString());

        firebaseBundle = new Parameter[1];
        firebaseBundle.SetValue(new Parameter("adType", adType.ToString()), 0);

        AdsManager.adPrint($"{action} - {adType}");
    }

    private void Bundle(EventActions action, AdFormat adFormat, string error)
    {
        metaBundle.Clear();
        metaBundle.Add(FirebaseAnalytics.ParameterAdFormat, adFormat.ToString());
        metaBundle.Add("error", error);

        firebaseBundle = new Parameter[2];
        firebaseBundle.SetValue(new Parameter(FirebaseAnalytics.ParameterAdFormat, adFormat.ToString()), 0);
        firebaseBundle.SetValue(new Parameter("error", error), 1);

        AdsManager.adPrint($"{action} - {adFormat} - {error}");
    }

    private void Bundle(EventActions action, AdFormat adFormat, string platform, string currency, double value)
    {
        metaBundle.Clear();
        metaBundle.Add(FirebaseAnalytics.ParameterAdFormat, adFormat.ToString());
        metaBundle.Add(FirebaseAnalytics.ParameterAdPlatform, platform);
        metaBundle.Add(FirebaseAnalytics.ParameterCurrency, currency);
        metaBundle.Add(FirebaseAnalytics.ParameterValue, value);

        firebaseBundle = new Parameter[4];
        firebaseBundle.SetValue(new Parameter(FirebaseAnalytics.ParameterAdFormat, adFormat.ToString()), 0);
        firebaseBundle.SetValue(new Parameter(FirebaseAnalytics.ParameterAdPlatform, platform), 1);
        firebaseBundle.SetValue(new Parameter(FirebaseAnalytics.ParameterCurrency, currency), 2);
        firebaseBundle.SetValue(new Parameter(FirebaseAnalytics.ParameterValue, value), 3);

        AdsManager.adPrint($"{action} - {adFormat} - {platform} - {currency} - {value}");
    }

    public static EventActions Parse(string name)
    {
        foreach (EventActions item in Enum.GetValues(typeof(EventActions)))
        {
            if (Enum.GetName(typeof(EventActions), item) == name) return item;
        }
        return EventActions.test; 
    }
}

public enum EventActions {

    adReceived, adFailed, adDisplayed, adFailedToShow,
    adBannerReceived, adBannerFailed, adBannerDisplayed,
    adInterstitialReceived, adInterstitialFailed, adInterstitialDisplayed,
    adRewardedReceived, adRewardedFailed, adRewardedDisplayed,
    test, adRequestedToDisplay, levelCompleted10, levelCompleted20, levelCompleted30, levelCompleted40, levelCompleted50,
    levelCompleted60, levelCompleted70, levelCompleted80, levelCompleted90, levelCompleted100
   
}