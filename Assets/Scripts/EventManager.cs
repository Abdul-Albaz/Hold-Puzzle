
using Facebook.Unity;
using Firebase.Analytics;

using System;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager>
{
    public async void logEvent(EventActions eventId)
    {
        FirebaseAnalytics.LogEvent(eventId.ToString());
        FB.LogAppEvent(eventId.ToString());
    }

    public async void logEvent(EventActions action, AdFormat adFormat, string platform, string currency, double value)
    {
        Parameter[] bundle = new Parameter[4]; ;

        bundle.SetValue(new Parameter(FirebaseAnalytics.ParameterAdFormat, adFormat.ToString()), 0);
        bundle.SetValue(new Parameter(FirebaseAnalytics.ParameterAdPlatform, platform), 1);
        bundle.SetValue(new Parameter(FirebaseAnalytics.ParameterCurrency, currency), 2);
        bundle.SetValue(new Parameter(FirebaseAnalytics.ParameterValue, value), 3);

        Dictionary<string, object> FBBundle = new();
        FBBundle.Add(FirebaseAnalytics.ParameterAdFormat, adFormat.ToString());
        FBBundle.Add(FirebaseAnalytics.ParameterAdPlatform, platform);
        FBBundle.Add(FirebaseAnalytics.ParameterCurrency, currency);
        FBBundle.Add(FirebaseAnalytics.ParameterValue, value);

        FirebaseAnalytics.LogEvent(action.ToString(), bundle);

        AdsManager.adPrint($"{bundle[0]} {bundle[1]} {bundle[2]} {bundle[3]} ");
        AdsManager.adPrint($"{adFormat} {platform} {currency} {value} ");
        FB.LogAppEvent(action.ToString(), parameters: FBBundle);
    }

    public static EventActions Parse(string name)
    {
        foreach (EventActions item in Enum.GetValues(typeof(EventActions)))
        {
            if (Enum.GetName(typeof(EventActions), item) == name) return item;
        }
        return EventActions.deneme; 
    }

    public void Deneme()
    {
        logEvent(EventActions.adDisplayed, AdFormat.interstitial, "ADMOB", "USD", 0.2);
        logEvent(EventActions.adInterstitialDisplayed, AdFormat.interstitial, "ADMOB", "USD", 0.2);
    }
}

public enum EventActions {

    adReceived, adFailed, adDisplayed,
    adBannerReceived, adBannerFailed, adBannerDisplayed,
    adInterstitialReceived, adInterstitialFailed, adInterstitialDisplayed,
    adRewardedReceived, adRewardedFailed, adRewardedDisplayed, deneme
}