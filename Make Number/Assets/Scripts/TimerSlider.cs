using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSlider : MonoBehaviour
{

    private Slider slider;

    private float elapsed;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void __Init__()
    {
        elapsed = 0f;
        slider.value = 1f;
    }

    void OnEnable()
    {
        elapsed = 0f;
        slider.value = 1f;
    }

    void Update()
    {
        if (GameManager.Instance.state != GameState.MainGame)
            return;

        if(slider.value == 0)
        {
            GameManager.Instance.GameOverStart();
        }

        elapsed += Time.deltaTime;

        slider.value = Mathf.Clamp01(1f - (elapsed / GameManager.Instance.duration));
    }

    public float GetElapsed()
    {
        return elapsed;
    }
}
