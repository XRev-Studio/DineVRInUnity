 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;
    private int currentPlayer;
    private bool isCoroutineRunning = true;
    public static Action<Player> OnPlayerNumberAssigned;
    //connect to server
  
   
    public void StartGame()
    {
        StartCoroutine(StartGameCourtine());
    }
    IEnumerator StartGameCourtine()
    {
        while (isCoroutineRunning)
        {
            if (PhotonNetwork.MasterClient != null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("IsMasterClient");
                    GameManager.instance.player = Player.one;
                    GameManager.instance.gameStarted = true;
                    StartCoroutine(SetRandomServerCourtine());
                }
                else
                {
                    Debug.Log("NotMaster");
                    GameManager.instance.player = Player.two;
                }
                Debug.Log("You Are Player : " + GameManager.instance.player);
                isCoroutineRunning = false;
                OnPlayerNumberAssigned.Invoke(GameManager.instance.player);
            }

            yield return null;
        }
    }
   
    IEnumerator SetRandomServerCourtine()
    {
        yield return new WaitForSeconds(5);
        SetRandomServer_net();
    }
    public void SetServer_net(int playerno)
    {
        photonView.RPC("GetSetServer", RpcTarget.AllBuffered, playerno);
    }
    [PunRPC]
    private void GetSetServer(int playerno)
    {
        currentPlayer = playerno;
        Debug.Log("ServeTurn : " + currentPlayer);
        photonView.RPC("UpdateCurrentPlayer", RpcTarget.AllBuffered, currentPlayer);
    }

    internal void SetPlayerTurn_net(int v)
    {
        photonView.RPC("UpdatePlayerTurn", RpcTarget.AllBuffered, v);
    }
    [PunRPC]
    void UpdatePlayerTurn(int playerNo)
    {
        GameManager.instance.updateRallyTurn_netCallBack(playerNo);
    }
    internal void StartRally_net()
    {
        photonView.RPC("UpdatePlayState", RpcTarget.AllBuffered, PlayState.Rally);
    }
    [PunRPC]
    void UpdatePlayState(PlayState state)
    {
        Debug.Log("PlayState : " + state.ToString());
        GameManager.instance.ruleManager.StartRally();
    }
    public void SetRandomServer_net()
    {
        
        int randomValue = UnityEngine.Random.Range(1, 3); 

        currentPlayer = randomValue;
        photonView.RPC("UpdateCurrentPlayer", RpcTarget.AllBuffered, currentPlayer);
    }
    [PunRPC]
    private void UpdateCurrentPlayer(int newPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            GameManager.instance.player = Player.two;
        }
        currentPlayer = newPlayer;
        Debug.Log("Server is being Updated_Net: " + currentPlayer);

        GameManager.instance.UpdateCurrentServer(currentPlayer);

    }
    internal void UpdateUserScoreNetwork(int playerOnePoints,int playerTwoPoints)
    {
        photonView.RPC("updateScore", RpcTarget.AllBuffered,playerOnePoints,playerTwoPoints);
    }
    [PunRPC]
    void updateScore(int playerOnePoints, int playerTwoPoints)
    {
        Debug.Log("ScoreUpdated \n PlayerOnePoints : " + playerOnePoints + "\n PlayerTwoPoints : " + playerTwoPoints);
        GameManager.instance.ruleManager.PlayerOnePoints = playerOnePoints;
        GameManager.instance.ruleManager.PlayerTwoPoints = playerTwoPoints;
        GameManager.instance.ruleManager.CheckIfRoundWon();
        GameManager.instance.UpdateUi();
    }

    internal void ConsectivePointAdd_net(int playerNo, int point)
    {
        photonView.RPC("AddConsectiveServePoint", RpcTarget.AllBuffered,playerNo,point);
    }
    [PunRPC]
    public void AddConsectiveServePoint(int playerNo, int point)
    {
        if(playerNo == 1 && GameManager.instance.player == Player.one)
        {
            GameManager.instance.ruleManager.ConsectivePoint++;
            return;
        }
        else if(playerNo == 2 && GameManager.instance.player == Player.two)
        {
            GameManager.instance.ruleManager.ConsectivePoint++;
            return;
        }
    }
    internal void GameEnded_Net()
    {
        photonView.RPC("GameEnd", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void GameEnd()
    {
        PhotonNetwork.LoadLevel(0);
    }
    internal void updateUI_Net()
    {
        photonView.RPC("UpdateUi", RpcTarget.AllBuffered,GameManager.instance.ruleManager.PlayerOnePoints,GameManager.instance.ruleManager.PlayerTwoPoints,GameManager.instance.ruleManager.CurrentRallyTurn,GameManager.instance.ruleManager.CurrentServer, GameManager.instance.ruleManager.currentRound,GameManager.instance.ruleManager.FirstRoundWinner,GameManager.instance.ruleManager.SecondRoundWinner,GameManager.instance.ruleManager.ThirdRoundWinner);
    }
    [PunRPC]
    void UpdateUi(int pop,int ptp, Player rallyturn,Player serveturn,int currentRound,Winner r1w,Winner r2w,Winner r3w)
    {
        GameManager.instance.SetSUI(pop,ptp,rallyturn,serveturn,currentRound,r1w,r2w,r3w);
    }
    
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect To Server");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Server");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("We Joined the Lobby");
        roomUI.SetActive(true);
    }

    public void InitializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

// LOAD Scene
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

//Create the room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room.");
        base.OnJoinedRoom();
    }

    internal void resetStats()
    {
        photonView.RPC("ResetStats", RpcTarget.AllBuffered, currentPlayer);
    }
    [PunRPC]
    public void ResetStats(int currentplayer)
    {
        GameManager.instance.ruleManager.CurrentServeNumberAttempt = 0;
        GameManager.instance.ruleManager.PlayerOnePoints = 0;
        GameManager.instance.ruleManager.PlayerTwoPoints = 0;
        GameManager.instance.ruleManager.playState = PlayState.serve;
    }
}
