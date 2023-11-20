using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    RuleManagerNew ruleManager;
    public ballState ballState;
    bool isCollidingWIthLand = false;
    private void Start()
    {
        ruleManager = GameManager.instance.ruleManager;
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "LP1" || collision.gameObject.tag == "LP2" || collision.gameObject.tag == "Net"|| collision.gameObject.tag == "Table")
        {
            isCollidingWIthLand = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Racket")
        {
            var racket = collision.gameObject.GetComponent<Racket>();
            ruleManager.LastCollidedRacket = racket.player;
            ruleManager.OnBallCollisonWithRacket(racket.player);
        }
        if(collision.gameObject.tag == "LP1")
        {
            ruleManager.CurrentBallSurfaceArea = Area.playerOneSide;
            ballState = ballState.land;
           // Debug.Log("Ball is Grounded");
        }
        if(collision.gameObject.tag == "LP2")
        {
            ruleManager.CurrentBallSurfaceArea = Area.PlayerTwoSide;
            ballState = ballState.land;
            //Debug.Log("Ball is Grounded");
        }
        if(collision.gameObject.tag == "Net")
        {
            ruleManager.CurrentBallSurfaceArea = Area.Net;
            ballState = ballState.land;
            //Debug.Log("Ball is Grounded");
        }
        if (collision.gameObject.tag == "Floor")
        {
            ruleManager.CurrentBallSurfaceArea = Area.Floor;
            ballState = ballState.land;
            //Debug.Log("Ball is Grounded");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AP1")
        {
            ballState = ballState.air;
            //Debug.Log("Ball is in Air");
            ruleManager.CurrentBallInAirArea = Area.playerOneSide;
        }
        if(other.gameObject.tag == "AP2")
        {
            ballState = ballState.air;
            //Debug.Log("Ball is in Air");
            ruleManager.CurrentBallInAirArea = Area.PlayerTwoSide;
        }
    }
}
public enum ballState
{
    air,land
}