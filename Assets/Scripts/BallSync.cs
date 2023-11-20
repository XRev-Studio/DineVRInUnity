using UnityEngine;
using Photon.Pun;

public class BallSync : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

}
