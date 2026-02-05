using UnityEngine;

public class NoAdsManager : MonoBehaviour
{
    public static NoAdsManager Instance;

    private const string NO_ADS_KEY = "NO_ADS_PURCHASED";
    public bool HasNoAds { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Load()
    {
        HasNoAds = PlayerPrefs.GetInt(NO_ADS_KEY, 0) == 1;
    }

    public void SetNoAdsPurchased()
    {
        HasNoAds = true;
        PlayerPrefs.SetInt(NO_ADS_KEY, 1);
        PlayerPrefs.Save();
        BannerAd.Instance.RefreshBanner();
    }

    public void DebugReset()
    {
        HasNoAds = false;
        PlayerPrefs.DeleteKey(NO_ADS_KEY);
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    public void Debug_ForceNoAdsPurchased()
    {
        Debug.Log("[DEBUG] Force No Ads Purchased");

        HasNoAds = true;

        PlayerPrefs.SetInt("NO_ADS", 1);
        PlayerPrefs.Save();

        // 🔥 배너 즉시 제거
        if (BannerAd.Instance != null)
        {
            BannerAd.Instance.RefreshBanner();
        }

        Debug.Log("[DEBUG] All ads disabled");
    }
#endif
}
