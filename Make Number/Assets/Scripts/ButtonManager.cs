using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StageNext()
    {
        GameManager.Instance.__Init__();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OnClickContinueWithAd()
    {
        AdsManager.Instance.ShowRewardedAd(
        onCompleted: () =>
        {
            GameManager.Instance.Continue_Init_();
        },
        onFailed: () =>
        {
            GameManager.Instance.NoContiue();
        }
    );
    }

    public void NoContinueButton()
    {
        GameManager.Instance.NoContiue();
    }
}
