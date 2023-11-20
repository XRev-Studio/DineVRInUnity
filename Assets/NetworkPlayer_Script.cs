using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer_Script : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GetComponent <Scoring>().hitter = "player (2)";
        }  
    }

}
