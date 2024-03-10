using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Date : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI dateText;
    void Start()
    {
        System.DateTime dateTime = System.DateTime.Now;
        dateText.text = dateTime.ToString("dd/MM/yyyy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
