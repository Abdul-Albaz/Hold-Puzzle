using Facebook.Unity;
using UnityEngine;

public class MetaManager : MonoBehaviour
{
    void Awake()
    {
        if (!FB.IsInitialized) FB.Init(InitCallback, OnHideUnity);
        else AdsManager.adPrint("Facebook not Initialized");
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            AdsManager.adPrint("Facebook is Initialized");
        }
        else AdsManager.adPrint("Failed to Initialize the Facebook SDK");
    }

    private void OnHideUnity(bool isGameShown) { }
}
