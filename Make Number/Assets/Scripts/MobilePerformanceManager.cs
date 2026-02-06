using UnityEngine;

public class MobilePerformanceManager : MonoBehaviour
{
    private static MobilePerformanceManager instance;

    void Awake()
    {
        // 중복 방지
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InitPerformance();
    }

    void InitPerformance()
    {
        // 🔒 프레임 고정
        Application.targetFrameRate = 60;

        // ❌ 모바일에서는 VSync 끄기
        QualitySettings.vSyncCount = 0;

#if UNITY_ANDROID || UNITY_IOS
        // 화면 꺼짐 방지 (게임 중)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
    }
}