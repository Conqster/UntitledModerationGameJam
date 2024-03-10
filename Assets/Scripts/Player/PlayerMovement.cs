using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    Vector3 velocity;

    public Rigidbody beverageObject;
    [SerializeField] float throwForce = 200f;
   
    private void Start()
    {
        
    }
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(beverageObject != null)
            {
                beverageObject.useGravity = true;
                beverageObject.isKinematic = false;

                beverageObject.AddForce(transform.forward * throwForce, ForceMode.Impulse);

                beverageObject = null;
            }
        }
    }
}
