using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeverageInteraction : MonoBehaviour
{
    [SerializeField] GameObject beveragePrefab;
    [SerializeField] Transform attachpoint;
    [SerializeField] PlayerMovement player;

    [SerializeField] bool inRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void Update()
    {
        if(inRange)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Making Object");
                // grab drink
                var obj = Instantiate(beveragePrefab, attachpoint.position, Quaternion.identity, attachpoint);
                var rb = obj.GetComponent<Rigidbody>();
                player.beverageObject = rb;
            }
        }
    }
}
