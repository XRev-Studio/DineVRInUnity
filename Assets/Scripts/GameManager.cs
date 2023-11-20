using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball Ball;

    public static GameManager instance;

    public RuleManagerNew ruleManager;

    public NetworkManager networkManager;

    public UiManager uiManager;

    public Hand hand;
    internal bool gameStarted;
    public Player player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        networkManager.StartGame();
    }
    internal void SetServer(int PlayerNo)
    {
        networkManager.SetServer_net(PlayerNo);
    }
    internal void SetToss(bool canToss)
    {
        hand.SetCanToss(canToss);
    }
    internal void UpdateCurrentServer(int currentPlayer)
    {
        ruleManager.playState = PlayState.serve;
        hand.SetCanToss(true);
        //uiManager.UpdatePanel(currentPlayer);
        if(currentPlayer == 1)
        {
            ruleManager.CurrentServer = Player.one;
            Debug.Log("Current Server is : " + ruleManager.CurrentServer);
            if(player == Player.one)
            {
                hand.cantoss = true; Debug.Log("Player One Can Toss");
            }
            else
            {
                hand.cantoss = false; Debug.Log("Player two Cannot Toss");
            }
        }
        else if(currentPlayer == 2)
        {
            ruleManager.CurrentServer = Player.two;
            Debug.Log("Current Server is : " + ruleManager.CurrentServer);
            if (player == Player.two)
            {
                hand.cantoss = true; Debug.Log("Player Two Can Toss");
            }
            else
            {
                hand.cantoss = false; Debug.Log("Player one Cannot Toss");
            }
        }
        UpdateUi();
    }

    internal void StartRally()
    {
        ruleManager.playState = PlayState.Rally;
        networkManager.StartRally_net();
    }

    internal void SetPlayerRallyTurn(int v)
    {
        networkManager.SetPlayerTurn_net(v);
    }
    internal void updateRallyTurn_netCallBack(int playerNo)
    {
        if(playerNo == 1)
        {
            ruleManager.CurrentRallyTurn = Player.one;
        }
        else
        {
            ruleManager.CurrentRallyTurn = Player.two;
        }
        Debug.Log("It's Player : " + playerNo + " Turn");
    }

    internal void UpdateScore(int playerNo, int Score)
    {
        if(playerNo == 1)
        {
            ruleManager.PlayerTwoPoints += Score;
        }
        else
        {
            ruleManager.PlayerOnePoints += Score;
        }
        networkManager.UpdateUserScoreNetwork(ruleManager.PlayerOnePoints,ruleManager.PlayerTwoPoints);
        
    }
    public void UpdateUi()
    {
        networkManager.updateUI_Net();
    }
    public void SetSUI(int pop, int ptp, Player rallyturn, Player serveturn, int currentRound, Winner r1w, Winner r2w, Winner r3w)
    {
        if(ruleManager.playState == PlayState.serve)
        {
         uiManager.UpdatePanels(pop, ptp, serveturn.ToString(),currentRound,r1w,r2w,r3w);
        }
        else
        {
            uiManager.UpdatePanels(pop, ptp, rallyturn.ToString(), currentRound, r1w, r2w, r3w);
        }
    }

    internal void OnGameEnded()
    {
        networkManager.GameEnded_Net();
    }

    internal void UpdateConsectivePointOnServe(int PlayerNo, int Point)
    {
        networkManager.ConsectivePointAdd_net(PlayerNo, Point);
    }
  
}
    

public enum Player
{
    one,two
}
public enum Winner
{
    None,one,two
}
public enum Area
{
    none,playerOneSide,PlayerTwoSide,Net,Floor
}