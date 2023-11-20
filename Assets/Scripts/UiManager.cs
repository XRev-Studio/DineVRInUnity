using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public List<TextMeshProUGUI> Player1ScoreText;
    public List<TextMeshProUGUI> Player2ScoreText;
    public List<TextMeshProUGUI> PlayerTurnText;
    public List<TextMeshProUGUI> CurrentRound;
    public List<TextMeshProUGUI> r1w,r2w,r3w;
    internal void UpdatePanels(int player1Score, int player2score, string playerturnNo,int currentRound, Winner r1, Winner r2, Winner r3)
    {
        foreach(var pOS in Player1ScoreText)
        {
            pOS.text = "Player 1 Score : " + player1Score;
        }
        foreach(var ptS in Player2ScoreText)
        {
            ptS.text = "Player 2 Score : " + player2score;
        }
        foreach(var pt in PlayerTurnText)
        {
            pt.text = "Player Turn : " + playerturnNo;
        } 
        foreach(var cr in CurrentRound)
        {
            cr.text = "Current Round : " + currentRound;
        } 
        foreach(var ri in r1w)
        {
            ri.text = "Round One Winner : " + r1;
        } 
        foreach(var rt in r2w)
        {
            rt.text = "Round Two Winner : " + r2;
        } 
        foreach(var rth in r3w)
        {
            rth.text = "Round Three Winner : " + r3;
        }
    }
}
