using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RealTimeNews : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void FixedUpdate()
    {
        text.text = DateTime.Now.Hour + " : " + DateTime.Now.Minute;
    }
}
