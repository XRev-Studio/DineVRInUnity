using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Racket : MonoBehaviour
{
    public Transform racketTransform;
    public float racketHitForce = 20f; // Adjust for racket hit strength
    public float minRacketSpeed = 1f;  //Minimum racket speed
    public float maxRacketSpeed = 10f;// Maximum racket speed
    public bool AllowBallSpin = false;

    Rigidbody ballRigidbody;
    Rigidbody rb;
    public float spinFactor = 0.5f;
    XRBaseInteractor interactor;

    public Player player;
    public bool automove = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MoveObject());
        NetworkManager.OnPlayerNumberAssigned += AssignPlayerRacket;
    }

    private void AssignPlayerRacket(Player obj)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
           player = obj;
        }
        else
        {
            if(obj == Player.two)
            {
                player = Player.one;
            }
            else
            {
                player = Player.two;
            }
        }
    }

    private void OnEnable()
    {
        GetComponent<XRGrabNetworkInteractable>().onSelectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        GetComponent<XRGrabNetworkInteractable>().onSelectEntered.RemoveListener(OnSelectEntered);
       
    }

    private void OnSelectEntered(XRBaseInteractor interactor)
    {
        player = GameManager.instance.player;
        this.interactor = interactor;
    }
  
    // Called when the racket hits the ball
    public void HitBall(Vector3 racketVelocity)
    {

        float speed = Mathf.Clamp(racketVelocity.magnitude, minRacketSpeed, maxRacketSpeed);

        // Calculate the hit direction based on the racket's velocity
        Vector3 hitDirection = racketVelocity.normalized;

        if (AllowBallSpin)
        {
            ballRigidbody.gameObject.GetComponent<Ball>().maxAngVel = 200;
        }
        // Apply force to the ball based on the hit strength and direction
        Vector3 force = hitDirection * (racketHitForce * speed);
        // racketlog.text = "Force : " + force + " , " + "Speed : " + speed;
        ballRigidbody.AddForce(force, ForceMode.Impulse);


        // Calculate and apply spin to the ball based on racket's motion
        Vector3 racketAngularVelocity = rb.angularVelocity;
        Vector3 spinTorque = racketAngularVelocity * spinFactor;
        ballRigidbody.AddTorque(spinTorque, ForceMode.Impulse);

        if (interactor != null)
        {
            interactor.GetComponent<ActionBasedController>().SendHapticImpulse(0.2f, 0.1f); // Adjust intensity and duration as needed
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            var ball = collision.gameObject.GetComponent<Ball>();
            //ball.TransferOwnerShip(GameManager.instance.player);
            ballRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            //ball.RemoveAllForces();
            HitBall(rb.velocity);
        }
    }
    private bool isMovingForward = true;
    private float startPositionZ = -1.0f;
    private float endPositionZ = -0.75f;
    private float speed = 2.0f;


    private IEnumerator MoveObject()
    {
        if (automove)
        {
            while (true) // Infinite loop
            {
                float targetZ = isMovingForward ? endPositionZ : startPositionZ;

                while (Mathf.Abs(transform.position.z - targetZ) > 0.01f)
                {
                    float step = speed * Time.deltaTime;
                    transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, targetZ, step));
                    yield return null;
                }

                // Change direction
                isMovingForward = !isMovingForward;

                // Add a delay before reversing the direction
                yield return new WaitForSeconds(0); // Adjust the delay duration as needed
            }
        }
        else
        {
            yield return null;
        }
    }

}
