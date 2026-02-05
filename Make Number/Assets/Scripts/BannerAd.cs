using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class BannerAd : MonoBehaviour
{
    public static BannerAd Instance;

    private BannerView bannerView;

#if UNITY_ANDROID
    private const string BANNER_ID = "ca-app-pub-9548284037151614/3651660611";
#endif

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += (_, __) =>
        {
            RefreshBanner(); // ⭐ 씬 바뀔 때마다 재부착
        };
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= (_, __) => { };
    }

    // ⭐ 핵심 함수 (이거 하나면 끝)
    public void RefreshBanner()
    {
        if (NoAdsManager.Instance.HasNoAds)
        {
            bannerView?.Destroy();
            bannerView = null;
            return;
        }

        // 이미 있으면 일단 제거 (AdMob View 꼬임 방지)
        bannerView?.Destroy();

        bannerView = new BannerView(
            BANNER_ID,
            AdSize.Banner,
            AdPosition.Bottom
        );

        bannerView.OnBannerAdLoaded += () =>
        {
            bannerView.Show();
        };

        bannerView.LoadAd(new AdRequest());
    }
}