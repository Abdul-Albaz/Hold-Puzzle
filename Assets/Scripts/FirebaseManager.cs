using Firebase;
using UnityEngine;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Collections.Generic;

public class FirebaseManager : MonoBehaviour
{
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    public void Awake()
    {
        AdsManager.adPrint("Firebase init");
        FirebaseApp.Create();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) InitializeFirebase();
            else AdsManager.adPrint("Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
    }

    void InitializeFirebase()
    {
        AdsManager.adPrint("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        Dictionary<string, object> defaults = new();
        defaults.Add("shouldShowAds", false);
        defaults.Add("adTimeLimit", 60);

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
        ConfigSettings settings = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
        settings.MinimumFetchInternalInMilliseconds = 0;

        FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(settings);

        FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync()
            .ContinueWithOnMainThread(task =>
            {
                AdsManager.adPrint("RemoteConfig configured and ready!");
                fetchData();
            });
    }

    void fetchData()
    {
        AdsManager.adPrint("fetchData");
        AdsManager.Instance.shouldShowAds = FirebaseRemoteConfig.DefaultInstance.GetValue("shouldShowAds").BooleanValue;
        AdsManager.timeLimit = FirebaseRemoteConfig.DefaultInstance.GetValue("adTimeLimit").LongValue;
        AdsManager.adPrint($"shouldShowAds: {AdsManager.Instance.shouldShowAds} - adTimeLimit: {AdsManager.timeLimit}");
    }
}
