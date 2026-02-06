using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    ButtonClick,
    DragSuccess,
    GameClear,
    GameOverTimer,
    GameOver
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [Header("SFX List")]
    [SerializeField] private List<SFXData> sfxList;

    private Dictionary<SFXType, SFXData> sfxDict;
    private AudioSource sfxSource;

    private void Awake()
    {
        // 싱글톤
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource 세팅
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f; // ⭐ 2D 사운드 (틱소리 방지)

        // Dictionary 변환
        sfxDict = new Dictionary<SFXType, SFXData>();
        foreach (var sfx in sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.type))
                sfxDict.Add(sfx.type, sfx);
        }
    }

    // =============================
    // 외부에서 호출하는 핵심 함수
    // =============================
    public void Play(SFXType type)
    {

        if (!sfxDict.TryGetValue(type, out var data))
        {
            Debug.LogWarning($"[SFXManager] 등록되지 않은 SFX: {type}");
            return;
        }

        sfxSource.loop = data.loop;
        sfxSource.volume = data.volume;
        sfxSource.PlayOneShot(data.clip);
    }

    public void Stop()
    {
        sfxSource.Stop();
    }
}
