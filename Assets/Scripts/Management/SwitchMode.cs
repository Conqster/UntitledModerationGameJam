using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchMode : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Cam1;
    [SerializeField] private Camera Cam2;
    //[SerializeField] private Camera Cam3;
    [SerializeField] private GameObject Id;
    private void Start()
    {
        Cam2.gameObject.SetActive(false);
        //Cam3.gameObject.SetActive(false);
        Id.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if(Cam1.activeInHierarchy == true)
        {
            if(Input.GetKeyDown(KeyCode.E)) {

                Cam1.gameObject.SetActive(false);
                Cam2.gameObject.SetActive(true);
                Id.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


            }
            
            
        } 
    }

    private void Update()
    {
        
    }
    public void Leave()
    {
        Cam1.gameObject.SetActive(true);
        Cam2.gameObject.SetActive(false);
        Id.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void Accept()
    {

    }
    public void Deny()
    {

    }
}
