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

    public Queue<Command> commandQueue;

    public GameObject PlayerMenu;

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

        commandQueue = new Queue<Command>();
    }

    void Update() {
        if(commandQueue.Count != 0) {
            enemyCommand(commandQueue.Dequeue());
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

    public void EndTurn() {
        UpdateActiveMenu();
        StartCoroutine(ExecuteEnemyTurn());
    }

    public void StartEnemyTurn() {
        StartCoroutine(ExecuteEnemyTurn());
    }

    IEnumerator ExecuteEnemyTurn() {
        yield return new WaitForEndOfFrame(); // Wait for the current frame to end
        EnemyAi ai = new EnemyAi();
        ai.enemyTurn(this);
        // Trigger enemy AI's turn
        // Optionally, you can notify the TurnManager that the enemy turn is completed here
        // depending on your game logic
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

    /*********************************
        Enemy Turn
    *********************************/
    public void enemyCommand(Command command) {
        Movement.moveEnemy(command.startSpace, command.moveSpace);
    }

    public void startPlayerTurn() {
        Debug.Log("Start Player Turn");
        ResetVals();
        UpdateActiveMenu();
    }

    public void UpdateActiveMenu() {
        PlayerMenu.SetActive(!PlayerMenu.activeSelf);
    }
}
