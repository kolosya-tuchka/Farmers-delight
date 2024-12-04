using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class FPSCount : MonoBehaviour
{
    public Text FPSText;
    public float FPSTextUpdateRate;
    float _FPSTextUpdateTimer;
    void Start()
    {
        _FPSTextUpdateTimer = 0;
    }

    void Update()
    {
        if (_FPSTextUpdateTimer <= 0)
        {
            FPSText.text = (Mathf.Round(1f / Time.deltaTime)).ToString();
            _FPSTextUpdateTimer = FPSTextUpdateRate;
        }
        else
        {
            _FPSTextUpdateTimer -= Time.deltaTime;
        }
    }
}
