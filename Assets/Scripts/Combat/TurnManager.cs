using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    List<Vector3Int> playerCoords;
    List<int> playerMovement;
    List<bool> playerAction;
    List<bool> playerTurnComplete;
    bool allPlayerTurnsUsed;

    private void Start() {
        playerCoords = new List<Vector3Int>();
        playerMovement = new List<int>(); //Movement speed left
        playerAction = new List<bool>(); //False - Action not use, True - Action Used
        playerTurnComplete = new List<bool>(); //False - Player can move or do an action, True - Action Used and Moved Used

        allPlayerTurnsUsed = false;

        foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players) {
            playerCoords.Add(playerInfo.Key);
            playerMovement.Add(playerInfo.Value.move);
            playerAction.Add(false);
            playerTurnComplete.Add(false);
        }
    }


    /*********************************
        Update Player Turn Stats
    *********************************/

    public void Player_Move(Vector3Int playerPos, int distance, Vector3Int newPos) {
        int playerIndex = playerCoords.FindIndex(a => a == playerPos);

        playerCoords[playerIndex] = newPos;

        playerMovement[playerIndex] = playerMovement[playerIndex] - distance;

        if(playerAction[playerIndex] == true && playerMovement[playerIndex] == 0) {
            playerTurnComplete[playerIndex] = true;
            CheckPlayerTurn();
        }
    }

    public void Player_SoftAction(Vector3Int playerPos) {
        int playerIndex = playerCoords.FindIndex(a => a == playerPos);

        playerAction[playerIndex] = true;

        if(playerMovement[playerIndex] == 0) {
            playerTurnComplete[playerIndex] = true;
            CheckPlayerTurn();
        }
    }

    public void Player_HardAction(Vector3Int playerPos) {
        int playerIndex = playerCoords.FindIndex(a => a == playerPos);

        playerMovement[playerIndex] = 0;
        playerAction[playerIndex] = true;
        playerTurnComplete[playerIndex] = true;

        CheckPlayerTurn();
    }

    void CheckPlayerTurn() {
        foreach(bool info in playerTurnComplete) {
            if(!info) {
                return;
            }
        }

        allPlayerTurnsUsed = true;
        Debug.Log("PLAYER TURN HAS BEEN COMPLETED");
        ResetVals();
    }

    public void ResetVals() {
        List<int> temp = new List<int>();
        foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
            temp.Add(playerInfo.Value.move);
        }

        for(int i = 0; i < playerCoords.Count; i++) {
            playerMovement[i] = temp[i];
            playerAction[i] = (false);
            playerTurnComplete[i] = (false);
        }
    }

    /*********************************
        Get Player Turn Stats
    *********************************/

    public int getMovementLeft(Vector3Int playerPos) {
        return playerMovement[playerCoords.FindIndex(a => a == playerPos)];
    }

    public bool getActionUse(Vector3Int playerPos) {
        return playerAction[playerCoords.FindIndex(a => a == playerPos)];
    }



















    //bool playerTurn = true;
    //List<bool> playerTurnsTaken; //False player has turn still, True PLayer has no turn
    //List<Vector3Int> playerCoord;

    //private void Start() {
    //    playerCoord = new List<Vector3Int>();
    //    playerTurnsTaken = new List<bool>();

    //    foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
    //        playerCoord.Add(playerInfo.Key);
    //        playerTurnsTaken.Add(false);
    //    }
    //}

    //void Update() {
    //    if(playerTurn){

    //    }
    //    else {
    //        FindObjectOfType<InputManager>().gameObject.SetActive(false);
    //    }

    //    bool allTurntaken = true;
    //    foreach(var turn in playerTurnsTaken) {
    //        if(!turn) {
    //            allTurntaken = false;
    //        }
    //    }

    //    if(allTurntaken) {
    //        allTurntaken = false;
    //        playerTurn = false;

    //        foreach(var turn in playerTurnsTaken) {
    //            if(!turn) {
    //                playerTurn = false;
    //            }
    //        }

    //        FindAnyObjectByType<EnemyAi>().transform.GetComponent<EnemyAi>().enemyTurn();
    //    }
    //}

    //public void EnemyturnTaken() {
    //    playerTurn = true;
    //    FindObjectOfType<InputManager>().gameObject.SetActive(true);

    //    playerCoord = new List<Vector3Int>();
    //    playerTurnsTaken = new List<bool>();

    //    foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players) {
    //        playerCoord.Add(playerInfo.Key);
    //        playerTurnsTaken.Add(false);
    //    }

    //    Debug.LogWarning("Player Turn");
    //}

    //public void playerTookTurn(Vector3Int playerPos) {
    //    int index = playerCoord.IndexOf(playerPos);

    //    if(index == -1) {
    //        Debug.LogWarning("TurnManager - playerPos not found");
    //        return;
    //    }

    //    playerTurnsTaken[index] = true;
    //}
}
