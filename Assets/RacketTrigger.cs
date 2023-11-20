using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class RacketTrigger : MonoBehaviour
{
    public float racketSpeedFactor = 1.0f;
    public Rigidbody racketRb;
    private Vector3 previousPosition;
    public Vector3 pVelocity;
    //public float testingSpeed = 2.0f;

    private void Start()
    {
        racketRb = GetComponent<Rigidbody>();
        //racketRb.AddForce(- transform.forward * testingSpeed);
        previousPosition = transform.position;
    }

    private void Update()
    {
        //pVelocity = previousPosition - transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");

        if (other.tag == "Ball")
        {
            ////from two days ago
            //Rigidbody rb = other.GetComponent<Rigidbody>(); //addforce function for the rb or velocity
            ////This is for calculating the ball speed, as dicussed two days ago
            //Vector3 racketVelocity = racketSpeedFactor * pVelocity; 
            //Vector3 ballVelocity = rb.velocity - racketVelocity;
            //Vector3 collisionNormal = transform.forward.normalized;
            //Vector3 reflectionDirection = ballVelocity - 2 * Vector3.Dot(ballVelocity, collisionNormal) * collisionNormal;
            //rb.velocity = reflectionDirection;

            // Get the contact point
            // Calculate the collision normal
            //Vector3 collisionNormal = (other.transform.position - transform.position).normalized;

            // Calculate the direction in which the ball should move after collision
            //Vector3 reflectionDirection = Vector3.Reflect(rb.velocity.normalized, collisionNormal);
            //Debug.Log(reflectionDirection.ToString());

            //adding force to the rigidbody
            //rb.velocity = reflectionDirection; //* GetComponent<Rigidbody>().velocity.magnitude;
        }
    }
}
