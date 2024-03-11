using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarStatus : MonoBehaviour
{

    [SerializeField] private bool isInBar = false;

    public bool IsinBar { get { return isInBar; } }


    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            isInBar = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            isInBar = false;
    }

}
