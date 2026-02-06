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
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif

    private bool adsInitialized = false;

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    private bool rewardedLoading = false;
    private bool interstitialLoading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); return; }

        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        InitAdsIfNeeded(() =>
        {
            // ✅ 초기화 끝나면 미리 로드
            LoadRewarded();
            LoadInterstitial();
        });
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

    // =========================
    // Preload
    // =========================

    public void LoadRewarded()
    {
        if (!adsInitialized || rewardedLoading) return;
        if (rewardedAd != null) return; // 이미 있으면 패스 (필요시 만료 처리까지 해도 됨)

        rewardedLoading = true;

        RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (ad, error) =>
        {
            rewardedLoading = false;

            if (error != null || ad == null)
            {
                // 실패 시: 나중에 다시 시도(원하면 코루틴/딜레이)
                return;
            }

            rewardedAd = ad;

            // 닫히면 다음 광고 미리 로드
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                rewardedAd = null;
                LoadRewarded();
            };

            rewardedAd.OnAdFullScreenContentFailed += (err) =>
            {
                rewardedAd = null;
                LoadRewarded();
            };
        });
    }

    public void LoadInterstitial()
    {
        if (!adsInitialized || interstitialLoading) return;
        if (interstitialAd != null) return;

        interstitialLoading = true;

        InterstitialAd.Load(interstitialAdUnitId, new AdRequest(), (ad, error) =>
        {
            interstitialLoading = false;

            if (error != null || ad == null)
            {
                return;
            }

            interstitialAd = ad;

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                interstitialAd = null;
                LoadInterstitial();
            };

            interstitialAd.OnAdFullScreenContentFailed += (err) =>
            {
                interstitialAd = null;
                LoadInterstitial();
            };
        });
    }

    // =========================
    // Show
    // =========================

    public void ShowRewardedAd(Action onCompleted, Action onFailed)
    {
        if (!adsInitialized)
        {
            onFailed?.Invoke();
            return;
        }

        if (rewardedAd == null)
        {
            // 아직 로드 전/실패 상태
            LoadRewarded();
            onFailed?.Invoke();
            return;
        }

        rewardedAd.Show(reward =>
        {
            onCompleted?.Invoke();
        });
    }

    public void ShowInterstitialAd()
    {
        if (NoAdsManager.Instance.HasNoAds) return;

        if (!adsInitialized) return;

        if (interstitialAd == null)
        {
            LoadInterstitial();
            return;
        }

        interstitialAd.Show();
    }
}