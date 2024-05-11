using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static List<Turn> turns = new List<Turn>();
    bool turnInProgress = false;
    public static int turnIndex = 0;

    private void OnEnable()
    {
        EventBus<EndTurnEvent>.Subscribe(OnEndTurn);
    }
    
    private void OnDisable()
    {
        EventBus<EndTurnEvent>.Unsubscribe(OnEndTurn);
    }

    public void OnStartTurn()
    {
        if (turnInProgress)
        {
            Debug.LogError("Turn already in progress");
            return;
        }
        Debug.Log("Start turn");
        turnInProgress = true;
        EventBus<StartTurnEvent>.Raise(new StartTurnEvent(turns[turnIndex]));
    }

    public void OnEndTurn(EndTurnEvent e)
    {
        turnInProgress = false;
        turnIndex++;
        
        if (turnIndex >= turns.Count)
        {
            Debug.Log("Game over");
            return;
        }
        
        //Request new customers list
        EventBus<RequestNewCustomersListEvent>.Raise(new RequestNewCustomersListEvent());
    }
}

[System.Serializable]
public class Turn
{
    public int turnIndex;
    public int playTime;
    public int basicCustomerCount;
}
