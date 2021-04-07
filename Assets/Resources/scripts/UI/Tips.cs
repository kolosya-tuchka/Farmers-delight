using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : MonoBehaviour
{
    Text txt;
    void Start()
    {
        txt = GetComponent<Text>();
        UpdateTip(null);
    }

    public void UpdateTip(string text)
    {
        txt.text = text;
    }
}
