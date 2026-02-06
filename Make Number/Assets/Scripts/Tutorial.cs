using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    [SerializeField]
    private Text text;

    public void SetFirstChange()
    {
        text.text = "90";
    }

    public void SetSecondChange()
    {
        text.text = "87";
    }
}
