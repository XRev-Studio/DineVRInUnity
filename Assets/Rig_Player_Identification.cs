using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rig_Player_Identification : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            GetComponent <Scoring>().hitter = "player (1)";
        }  
    }
}
