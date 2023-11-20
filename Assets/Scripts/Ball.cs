using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Ball : MonoBehaviourPunCallbacks, IPunObservable
{

    Rigidbody rb;
 
    public float maxAngVel;
    public float lerpSpeed = 0.5f;
    public float tossUpForce = 10;

    public BallCollisionHandler BallCollisionHandler;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        BallCollisionHandler = GetComponent<BallCollisionHandler>();
    }
    private void Update()
    {
        if (maxAngVel > 1.0f)
        {
            maxAngVel -= lerpSpeed * Time.deltaTime;
        }
    }

    internal void TossBall(Vector3 position)
    {
        rb.position = position;

        // You may also want to reset the ball's velocity to ensure a consistent toss behavior
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * tossUpForce);
    }

    public void TransferOwnerShip(Player player)
    {
        if(player == Player.one)
        {
            if(photonView.Owner != PhotonNetwork.MasterClient)
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
        }
        else
        {
            if (photonView.Owner != PhotonNetwork.LocalPlayer)
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }

    void FixedUpdate()
    {
        rb.maxAngularVelocity = maxAngVel;
    }
    public void RemoveAllForces()
    {
        // Set the velocity to zero to remove any linear velocity.
        rb.velocity = Vector3.zero;

        // Set the angular velocity to zero to remove any angular velocity.
        rb.angularVelocity = Vector3.zero;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // If you're the owner of the ball, send its position and velocity
            stream.SendNext(rb.position);
            stream.SendNext(rb.velocity);
        }
        else
        {
            // If you're not the owner, receive the position and velocity
            rb.position = (Vector3)stream.ReceiveNext();
            rb.velocity = (Vector3)stream.ReceiveNext();
        }
    }

}
