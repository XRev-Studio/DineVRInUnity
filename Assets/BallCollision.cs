using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Racket"))
        {
            // Get the contact point
            ContactPoint contact = collision.contacts[0];

            // Calculate the collision normal
            Vector3 collisionNormal = contact.normal.normalized;

            // Calculate the direction in which the ball should move after collision
            Vector3 reflectionDirection = Vector3.Reflect(transform.forward, collisionNormal);

            // Apply the reflection direction as the new velocity of the ball
            GetComponent<Rigidbody>().velocity = reflectionDirection * GetComponent<Rigidbody>().velocity.magnitude;
        }
    }
}

