using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class Hand : MonoBehaviour
{
    private ActionBasedController actionBasedController;
    public bool cantoss = false;
    private void Start()
    {
        actionBasedController = GetComponent<ActionBasedController>();
    }

    private void Update()
    {
        if (actionBasedController.activateAction.action.triggered && cantoss)
        {
            GameManager.instance.Ball.gameObject.SetActive(true);
            GameManager.instance.Ball.TransferOwnerShip(GameManager.instance.player);
            Debug.Log("BallTossed from Player : " + GameManager.instance.player.ToString());
            GameManager.instance.Ball.TossBall(transform.position);
        }
    }

    internal void SetCanToss(bool v)
    {
        cantoss = v;
    }
}
