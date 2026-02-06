using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NOADButton : MonoBehaviour
{
    private void Start()
    {
        if(NoAdsManager.Instance.HasNoAds)
        {
            gameObject.SetActive(false);
        }
    }
}
