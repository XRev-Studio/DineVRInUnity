using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureOutPlayer : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Ball")
        {
            GetComponent<Scoring>().hitter = "Player (1)";
        }
    }
}
