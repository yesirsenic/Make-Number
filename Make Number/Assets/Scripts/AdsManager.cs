using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;


#if UNITY_ANDROID
    private string rewardedAdUnitId = "ca-app-pub-9548284037151614/8903987296";
    private string interstitialAdUnitId = "ca-app-pub-9548284037151614/5140738194";
#elif UNITY_IOS
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    // 👇 에디터용 
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif


    private bool adsInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
    }

    private void InitAdsIfNeeded(Action onInitialized = null)
    {
        if (adsInitialized)
        {
            onInitialized?.Invoke();
            return;
        }

        MobileAds.Initialize(_ =>
        {
            adsInitialized = true;
            onInitialized?.Invoke();
        });
    }


    public void ShowRewardedAd(Action onCompleted, Action onFailed)
    {
        InitAdsIfNeeded(() =>
        {
            RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (ad, error) =>
            {
                if (error != null || ad == null)
                {
                    onFailed?.Invoke();
                    return;
                }

                ad.Show(reward =>
                {
                    onCompleted?.Invoke();
                });
            });
        });
    }

    // =========================
    // Interstitial
    // =========================

    public void ShowInterstitialAd()
    {
        if (NoAdsManager.Instance.HasNoAds)
            return;

        InitAdsIfNeeded(() =>
        {
            InterstitialAd.Load(interstitialAdUnitId, new AdRequest(), (ad, error) =>
            {
                if (error != null || ad == null)
                    return;

                ad.Show();
            });
        });
    }

}