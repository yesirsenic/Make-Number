using UnityEngine;
using System;

public class InterstitialAdController : MonoBehaviour
{
    public static InterstitialAdController Instance;

    private const string KEY_LAST_PLAY_DATE = "LAST_PLAY_DATE";
    private const string KEY_CLEAR_COUNT = "CLEAR_COUNT";

    private int clearCountToday;
    private bool isFirstDay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitDayState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitDayState()
    {
        string today = DateTime.Now.ToString("yyyyMMdd");

        if (!PlayerPrefs.HasKey(KEY_LAST_PLAY_DATE))
        {
            // 완전 첫 플레이
            PlayerPrefs.SetString(KEY_LAST_PLAY_DATE, today);
            clearCountToday = 0;
            isFirstDay = true;
        }
        else
        {
            string lastDate = PlayerPrefs.GetString(KEY_LAST_PLAY_DATE);

            if (lastDate != today)
            {
                // 날짜 변경됨
                PlayerPrefs.SetString(KEY_LAST_PLAY_DATE, today);
                clearCountToday = 0;
                isFirstDay = false;
            }
            else
            {
                clearCountToday = PlayerPrefs.GetInt(KEY_CLEAR_COUNT, 0);
                isFirstDay = false;
            }
        }
    }

    public void OnGameClear()
    {
        clearCountToday++;
        PlayerPrefs.SetInt(KEY_CLEAR_COUNT, clearCountToday);

        if (ShouldShowAd())
        {
            AdsManager.Instance.ShowInterstitialAd();
        }
    }

    private bool ShouldShowAd()
    {
        if (isFirstDay)
        {
            // 첫날
            if (clearCountToday <= 3)
                return false;

            if (clearCountToday == 4)
                return true;

            return (clearCountToday - 4) % 2 == 0;
        }
        else
        {
            // 다음 날부터
            if (clearCountToday <= 2)
                return true;

            return (clearCountToday - 2) % 3 == 0;
        }
    }
}
