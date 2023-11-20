using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class Scoring : MonoBehaviour
{
//variables that are needed to get the basic rules started
//Determining who is hitting, we get information from DetermineHitter Script attached to player
    public string hitter;

//In here, we store the score values of the players
    public int playerOneScore;
    public int playerTwoScore;
    
//MaxScore in one Set to reach a Set
    public int maxScore;

//In here, we determine the set and the max amount of sets
    public int playerOneSet;
    public int playerTwoSet;

//In this variable, we store the Text displayed
    public Text ScoreText;


//initialize all values
    private void Start()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
        maxScore = 11;
        playerOneSet = 0;
        playerTwoSet = 0;
    }

//Rules (first just easy if-else clauses with colliders):
//First, we check if the Serve is correct. We have to alternate the serve after one serve is being hit
//Then, we constantly check if one player hits a winner or not, and increase the score of the player who hit the winner
//Then, we constantly check if one player hits straight out, and increase the opponents score
//Then, we constantly check if one player hits the net, and increase the opponents score


//Check if the ball is Out
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("Out"))
        {
            if (hitter == "Player(1)")
            {
                playerTwoScore ++;
            }
            else if (hitter == "Player(2)")
            {
                playerOneScore ++;
            }
        }
//Check if someone hits a Winner
        else if (other.CompareTag("Plane"))
        {
            if (hitter == "Player(1)")
            {
                playerOneScore ++;
            }
            else if (hitter == "Player(2)")
            {
                playerTwoScore ++;
            }
        }         
    }

    public void UpdateScore ()
    {
        ScoreText.text = "Score Player One: " + playerOneScore + "Score Player Two Score: " + playerTwoScore;
    }

    void Update()
    {
        UpdateScore();
    }
}

