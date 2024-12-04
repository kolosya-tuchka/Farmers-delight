using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class TMProCharactersLimit : MonoBehaviour
{
    public uint characterLimit = 16;
    private TextMeshProUGUI text;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.maxVisibleCharacters = (int)characterLimit;
    }
}
