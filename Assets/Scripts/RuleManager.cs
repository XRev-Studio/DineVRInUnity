using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    //Keeping track of Ball collisions
    public int ballCollisionCounter;
    //Who hits the ball
    public int lastHitBallPlayerID;
    public int currentBallArea = 0;
    //Move this to the Scorekeep script later
    public List<int> currentScore;
    //List to Store key value pairs for sets
    public List<KeyValuePair<int, int>> sets;
    //when ball falls out, we reset the ball where the transforms are
    public Transform [] playerBallSides;

    internal static void OnCollisonWithRacket(Player player)
    {
        throw new NotImplementedException();
    }

    //those variables help to alternate serves and spawn the ball
    public int currentServerID = 0;
    public int serveCounter = 0;
    public Transform ballTransform;

    private void Start()
    {
        currentScore = new List<int>();
        currentScore.Add(0);
        currentScore.Add(0);
        sets = new List<KeyValuePair<int, int>>();

    }


    public void TriggerEvent(bool entry, bool isArea = false, int playerID = 0)
    {
        if (isArea == true && entry == true)
        {
            currentBallArea = playerID;
            ballCollisionCounter = 0;
        }
        //if something is weird with the counting, change the table to the trigger and comment out below to line 46
        if (isArea == false)
        {
            ballCollisionCounter = 0;
            GiveScore();
            return;
        }

        ballCollisionCounter += 1;
        if (ballCollisionCounter >=2)
        {
            GiveScore();
        }
    }

    public void GiveScore ()
    {
        serveCounter += 1;
        if (serveCounter >=2 )
        {
            serveCounter = 0;
            if (currentServerID == 1)
            {
                currentServerID = 0;
            }
            else
            {
                currentServerID = 1;
            } 
        }
        //ballTransform.position = playerBallSides[currentServerID].position;
        //Rule is broken, who to give a point to
        Debug.Log("Score updated " + currentBallArea);
        int winnerID = 0;
        int loserID = 1;
        if (currentBallArea == 1)
        {
            winnerID = 1;
            loserID = 0;
        }
        currentScore[winnerID] += 1;
        Debug.Log("Player 0 Score: " + currentScore[0] + " Player 1 Score: " + currentScore[1]);

        if (currentScore[winnerID] >= 11 && Mathf.Abs(currentScore[loserID] - currentScore[winnerID]) >= 2)
        {
            //increase set, set end
            sets.Add(new KeyValuePair<int, int>(currentScore[0], currentScore[1]));
            int[] setKeeper = new int [] { 0, 0 };
            //Dotkey is player 0, DotValue is Player 1
            foreach (KeyValuePair<int, int> temp in sets)
            {
                if (temp.Key > temp.Value)
                {
                    setKeeper[0] += 1; 
                }
                else
                {
                    setKeeper[1] += 1;
                }
            }

            //which one is bigger to get the winner
            if (sets.Count >= 3)
            {
                if (setKeeper[0] > setKeeper[1])
                {
                    //player 0 wins
                    Debug.Log("Player 0 wins");
                }
                else
                {
                    //player 1 wins
                    Debug.Log("Player 1 wins");
                }
            }

        }

    }
}
