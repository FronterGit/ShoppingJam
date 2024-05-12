using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public List<Turn> turns = new List<Turn>();
    bool turnInProgress = false;
    public static int turnIndex = 0;

    public static Func<List<Turn>> GetTurns;

    private void OnEnable() {
        EventBus<EndTurnEvent>.Subscribe(OnEndTurn);
        GetTurns += GetTurnsList;
    }

    private void OnDisable() {
        EventBus<EndTurnEvent>.Unsubscribe(OnEndTurn);
        GetTurns -= GetTurnsList;
    }

    public void OnStartTurn() {
        //Check if we have any more turns to play
        if (turnIndex >= turns.Count) {
            Debug.Log("No more turns to play");
            return;
        }

        if (turnInProgress) {
            Debug.LogError("Turn already in progress");
            return;
        }

        turnInProgress = true;
        EventBus<StartTurnEvent>.Raise(new StartTurnEvent(turns[turnIndex]));
    }

    public void OnEndTurn(EndTurnEvent e) {
        turnInProgress = false;
        turnIndex++;

        // check if a store manager should spawn
        if (turns[turnIndex - 1].shouldSpawnManager) {
            EventBus<RegionalManagerEvent>.Raise(new RegionalManagerEvent(0, turns[turnIndex - 1].expectedRevenue));
        }

        if (turnIndex >= turns.Count) {
            Debug.Log("Game over");
            return;
        }
    }

    public List<Turn> GetTurnsList() {
        return turns;
    }
}

[System.Serializable]
public class Turn {
    public int turnIndex;
    public int playTime;
    public int basicCustomerCount;
    [Header("Regional Manager")] public bool shouldSpawnManager = false;
    public int expectedRevenue = 0;
}