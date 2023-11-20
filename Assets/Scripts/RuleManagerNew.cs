using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleManagerNew : MonoBehaviour
{

    public PlayState playState;

    public Player LastCollidedRacket;
    public Player FirstServer;
    public Player CurrentServer;
    public Player CurrentRallyTurn;

    public Area CurrentBallInAirArea;
    public Area CurrentBallSurfaceArea;
    public Area LastBallSurfaceArea;
    public Area LastBallAirArea;

    public int CurrentServeNumberAttempt;
    public int ConsectivePoint = 0;
    public int ServeNumber = 1;

    public int PlayerOnePoints;
    public int PlayerTwoPoints;

    public int currentRound = 1;
    public Winner FirstRoundWinner, SecondRoundWinner, ThirdRoundWinner = Winner.None ;


    float racketballinterval = 0.5f;
    bool checkServe = false;
    private Coroutine serveCoroutine;

    public Coroutine RallyCourtine;

    internal void OnBallCollisonWithRacket(Player player)
    {
        //if (GameManager.instance != null)
        //{
        // GameManager.instance.UpdateUi();
        //}
        if (racketballinterval < 2f) {
            Debug.Log("Interval is Less for Collision");
            return;
        }
        GameManager.instance.Ball.TransferOwnerShip(player);
       
        if (playState == PlayState.serve)
        {
            if(CurrentServer == player)
            {
                if (GameManager.instance.player == player) // check if ball is hitted by currect player's racket
                {
                    if (GameManager.instance.Ball.BallCollisionHandler.ballState == ballState.air) // Check if ball is not grounded
                    {
                       
                        CurrentServeNumberAttempt++;
                        checkServe = true;
                        Debug.Log("Current Serve Attempt From Player : " + player.ToString() +" IS  : " + CurrentServeNumberAttempt); ;
                        if (serveCoroutine != null)
                        {
                            StopCoroutine(serveCoroutine); // Stop the coroutine if it's already running.
                        }
                        serveCoroutine = StartCoroutine(CheckIfServeSuccesFull());
                        racketballinterval = 0f;

                       
                    }
                }
            }
        }
    }

    
    private void DisableServe()
    {
        GameManager.instance.SetToss(false);
        Debug.Log("Disabling Serve");
    }

    IEnumerator CheckIfServeSuccesFull()
    {
        Debug.Log("ServeChecking Courtine Started");
        bool firstCollisionOnPlayerOneSide = false;
        bool firstCollisionOnPlayerTwoSide = false;
        CurrentBallSurfaceArea = Area.none;
        float tsp1 = 0;
        float tsp2 = 0;
        while (true)
        {
            if (GameManager.instance.player == Player.one && CurrentServer == Player.one)
            {
                if (CurrentBallSurfaceArea == Area.playerOneSide)
                {
                    Debug.Log("Ball Entered Player : " + CurrentBallSurfaceArea.ToString());
                    firstCollisionOnPlayerOneSide = true;
                    tsp1 += Time.deltaTime;
                    if(tsp1 > 20f)
                    {
                        Debug.Log("Serve Un-Successful Ball Steady p1s");
                        OnServeUnScussesful();
                        break;
                    }
                }
                if (CurrentBallSurfaceArea == Area.Net)
                {
                    Debug.Log("Serve Un-Successful Collided With Net");
                    OnServeUnScussesful();
                    break;
                }
                if (CurrentBallSurfaceArea == Area.Floor)
                {
                    Debug.Log("Serve Un-Successful Collided With Floor/Plane");
                    OnServeUnScussesful();
                    break;
                }
                if (firstCollisionOnPlayerOneSide && CurrentBallSurfaceArea == Area.PlayerTwoSide)
                {
                    Debug.Log("ServeSuccess From Player : " + CurrentServer.ToString());
                    playState = PlayState.Rally;
                    DisableServe();
                    if (CurrentServer == Player.one)
                    {
                        GameManager.instance.SetPlayerRallyTurn(2);
                        CurrentRallyTurn = Player.two;
                    }
                    else
                    {
                        CurrentRallyTurn = Player.one;
                        GameManager.instance.SetPlayerRallyTurn(1);
                    }
                    GameManager.instance.StartRally();
                    break; // Exit the loop when the serve is successful.
                }
                else
                {
                    //OnServeUnScussesful();// If serve was has no scusses 
                }
            }
            else if (GameManager.instance.player == Player.two && CurrentServer == Player.two)
            {
              
                if (CurrentBallSurfaceArea == Area.PlayerTwoSide)
                {
                    //Debug.Log("Ball Entered Player : " + CurrentBallSurfaceArea.ToString());
                    firstCollisionOnPlayerTwoSide = true;
                    tsp2 += Time.deltaTime;
                    if (tsp2 > 20f)
                    {
                        Debug.Log("Serve Un-Successful Ball Steady p2s");
                        OnServeUnScussesful();
                        break;
                    }
                }
                if (CurrentBallSurfaceArea == Area.Net)
                {
                    Debug.Log("Serve Un-Successful Collided With Net");
                    OnServeUnScussesful();
                    break;
                }
                if(CurrentBallSurfaceArea == Area.Floor)
                {
                    Debug.Log("Serve Un-Successful Collided With Floor/Plane");
                    OnServeUnScussesful();
                    break;
                }
                if (firstCollisionOnPlayerTwoSide && CurrentBallSurfaceArea == Area.playerOneSide)
                {
                    Debug.Log("ServeSuccess From Player : " + CurrentServer.ToString());
                    DisableServe();
                    playState = PlayState.Rally;
                    if (CurrentServer == Player.one)
                    {
                        GameManager.instance.SetPlayerRallyTurn(2);
                        CurrentRallyTurn = Player.two;
                    }
                    else
                    {
                        CurrentRallyTurn = Player.one;
                        GameManager.instance.SetPlayerRallyTurn(1);
                    }
                    GameManager.instance.StartRally();
                    break; // Exit the loop when the serve is successful.
                }
                else
                {
                    //OnServeUnScussesful();// If serve was has no scusses 
                }
            }

            yield return null; // Yield to the Unity engine to avoid freezing the game.
        }

        
    }

    internal void CheckIfRoundWon()
    {
        if(currentRound <= 2)
        {
            if(currentRound == 1)
            {
                if (PlayerOnePoints > PlayerTwoPoints)
                {
                    int difference = Mathf.Abs(PlayerOnePoints - PlayerTwoPoints);
                    if(difference >= 2)
                    {
                        if (PlayerOnePoints >= 11)
                        {
                            currentRound++;
                            Debug.Log("R1W by Player 1");
                            FirstRoundWinner = Winner.one;
                            OnRoundWin();
                        }
                    }
                    
                }
                else if(PlayerOnePoints < PlayerTwoPoints)
                {
                    int difference = Mathf.Abs(PlayerOnePoints - PlayerTwoPoints);
                    if (difference >= 2)
                    {
                        if (PlayerTwoPoints >= 11)
                        {
                            currentRound++;
                            Debug.Log("R1W by Player 2");
                            FirstRoundWinner = Winner.two;
                            OnRoundWin();
                        }
                    }
                        
                }
            }
            else if(currentRound == 2)
            {
                if (PlayerOnePoints > PlayerTwoPoints)
                {
                    int difference = Mathf.Abs(PlayerOnePoints - PlayerTwoPoints);
                    if (difference >= 2)
                    {
                        if (PlayerOnePoints >= 11)
                        {
                            currentRound++;
                            Debug.Log("R2W by Player 1");
                            SecondRoundWinner = Winner.one;
                            OnRoundWin();
                        }
                    }
                       
                }
                else if (PlayerOnePoints < PlayerTwoPoints)
                {
                    int difference = Mathf.Abs(PlayerOnePoints - PlayerTwoPoints);
                    if (difference >= 2)
                    {
                        if (PlayerTwoPoints >= 11)
                        {
                            currentRound++;
                            Debug.Log("R2W by Player 2");
                            SecondRoundWinner = Winner.two;
                            OnRoundWin();
                        }
                    }
                        
                }
            }
           
        }
        else if(currentRound == 3)
        {
            if (PlayerOnePoints > PlayerTwoPoints)
            {
                int difference = Mathf.Abs(PlayerOnePoints - PlayerTwoPoints);
                if (difference >= 2)
                {
                    if (PlayerOnePoints >= 11)
                    {
                        Debug.Log("R3W by Player 1");
                        ThirdRoundWinner = Winner.one;
                        OnFinalRoundWin();
                    }
                }
                    
            }
            else if (PlayerOnePoints < PlayerTwoPoints)
            {
                int difference = Mathf.Abs(PlayerOnePoints - PlayerTwoPoints);
                if (difference >= 2)
                {
                    if (PlayerTwoPoints >= 11)
                    {
                        Debug.Log("R3W by Player 2");
                        ThirdRoundWinner = Winner.two;
                        OnFinalRoundWin();
                    }
                }
                    
            }
        }
    }

    private void OnFinalRoundWin()
    {
        GameManager.instance.UpdateUi();
        GameManager.instance.OnGameEnded();
    }

    private void OnRoundWin()
    {
        GameManager.instance.networkManager.resetStats();
        GameManager.instance.networkManager.SetRandomServer_net();
        GameManager.instance.UpdateUi();
    }

    

    public void StartRallyIfItsNotAlreadyRunning()
    {
        if (RallyCourtine == null)
        {
            RallyCourtine = StartCoroutine(RallyCheck());
        }
    }
    public void StartRally()
    {
        if (RallyCourtine != null)
        {
            StopCoroutine(RallyCourtine); // Stop the coroutine if it's already running.
        }
        RallyCourtine = StartCoroutine(RallyCheck());
    }

    IEnumerator RallyCheck()
    {
        Debug.Log("RallyCheck Strated");
        CurrentBallSurfaceArea = Area.none;
        float tps1 = 0;
        float tps2 = 0;
        while (true)
        {
           
            if (CurrentRallyTurn == Player.one)
            {
                bool firstCollisionOnPlayerTwoSide = false;
                bool ballCollidedWithNet = false;
                if (CurrentBallSurfaceArea == Area.PlayerTwoSide)
                {
                    firstCollisionOnPlayerTwoSide = true;
                    tps1 += Time.deltaTime;
                    if(tps1 >= 20)
                    {
                        Debug.Log("Delivery Failed By Player : " + CurrentRallyTurn.ToString() + "Steady On P1S");
                        ondeliveryUnSuccesFull(1);
                        break;
                    }
                }
                if (CurrentBallSurfaceArea == Area.Net)
                {
                    ballCollidedWithNet = true;
                    Debug.Log("Delivery Failed By Player : " + CurrentRallyTurn.ToString() + "Collided with net");
                    ondeliveryUnSuccesFull(1);
                    break;
                }
                if (CurrentBallSurfaceArea == Area.Floor)
                {
                    Debug.Log("Delivery Failed By Player : " + CurrentRallyTurn.ToString() + "Collided with Floor");
                    ondeliveryUnSuccesFull(1);
                    break;
                }
                if (firstCollisionOnPlayerTwoSide && !ballCollidedWithNet && CurrentBallSurfaceArea == Area.PlayerTwoSide)
                {
                    Debug.Log("DeliverySuccess : " + 1);
                    CurrentRallyTurn = Player.two;
                    firstCollisionOnPlayerTwoSide = false;
                    ballCollidedWithNet = false;
                    tps1 = 0;
                    tps2 = 0;
                    CurrentBallSurfaceArea = Area.none;
                    GameManager.instance.UpdateUi();
                }
            }
            else if (CurrentRallyTurn == Player.two)
            {
                bool FirstCollisonOnPlayerOneSide = false;
                bool ballCollidedWithNet = false;
                if (CurrentBallSurfaceArea == Area.playerOneSide)
                {
                    FirstCollisonOnPlayerOneSide = true;
                    tps2 += Time.deltaTime;
                    if (tps2 >= 20)
                    {
                        Debug.Log("Delivery Failed By Player : " + CurrentRallyTurn.ToString() + "Steady On P2S");
                        ondeliveryUnSuccesFull(2);
                        break;
                    }
                }
                if (CurrentBallSurfaceArea == Area.Net)
                {
                    ballCollidedWithNet = true;
                    Debug.Log("Delivery Failed By Player : " + CurrentRallyTurn.ToString() + "Collided with net");
                    ondeliveryUnSuccesFull(2);
                    break;
                }
                if (CurrentBallSurfaceArea == Area.Floor)
                {
                    Debug.Log("Delivery Failed By Player : " + CurrentRallyTurn.ToString() + "Collided with Floor");
                    ondeliveryUnSuccesFull(2);
                    break;
                }
                if (FirstCollisonOnPlayerOneSide && !ballCollidedWithNet && CurrentBallSurfaceArea == Area.playerOneSide)
                {
                    Debug.Log("DeliverySuccess : " + 2);
                    CurrentRallyTurn = Player.one;
                    FirstCollisonOnPlayerOneSide = false;
                    ballCollidedWithNet = false;
                    tps1 = 0;
                    tps2 = 0;
                    GameManager.instance.UpdateUi();
                }
            }
        yield return null;
        }
    }

    private void ondeliveryUnSuccesFull(int UnSuccesfullBy)
    {
        Debug.Log("Delivery Failed : " + UnSuccesfullBy);

        if (UnSuccesfullBy == 1)
        {
            GameManager.instance.UpdateScore(1, 1);
            GameManager.instance.UpdateConsectivePointOnServe(1, 1);
        }
        else if(UnSuccesfullBy == 2)
        {
            GameManager.instance.UpdateScore(2, 1);
            GameManager.instance.UpdateConsectivePointOnServe(2, 1);
        }


        if (ConsectivePoint == 2)
        {
            Debug.Log("Consective Point Scored");
            DisableServe();
            GameManager.instance.SetServer(UnSuccesfullBy);
            ConsectivePoint = 0;
        }
        else
        {
            if(CurrentServer == Player.one)
            {
                GameManager.instance.SetServer(1);
            }
            else
            {
                GameManager.instance.SetServer(2);
            }
        }
        
    }
    
    private void OnServeUnScussesful()
    {
        Debug.Log("Serve Number : " + ServeNumber + " , " + "Server Try Number : " + CurrentServeNumberAttempt + " , Consective Points : " + ConsectivePoint);
        if(CurrentServeNumberAttempt >= 2) //Change Server if 2 Try are Unscussesful
        {
            if(CurrentServer == Player.one)
            {
                GameManager.instance.UpdateScore(1, 1);
                GameManager.instance.UpdateConsectivePointOnServe(1, 1);
                CurrentServeNumberAttempt = 0;
                ServeNumber++;
            }
            else
            {
                GameManager.instance.UpdateScore(2, 1);
                GameManager.instance.UpdateConsectivePointOnServe(2, 1);
                CurrentServeNumberAttempt = 0;
                ServeNumber++;

            }
            if(ConsectivePoint == 2)
            {
                DisableServe();
                UpdateServer();
                ConsectivePoint = 0;
            }
           
        }
    }

    private void UpdateServer() // Update the server.
    {
        if (CurrentServer == Player.one)
        {
            GameManager.instance.SetServer(2);
        }
        else if(CurrentServer == Player.two)
        {
            GameManager.instance.SetServer(1);
        }
    }

    private void Update()
    {
        racketballinterval += Time.deltaTime;
    }
}
public enum PlayState
{
    serve,Rally
}