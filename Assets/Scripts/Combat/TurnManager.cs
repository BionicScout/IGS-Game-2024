using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    bool playerTurn = true;
    List<bool> playerTurnsTaken; //False player has turn still, True PLayer has no turn
    List<Vector3Int> playerCoord;

    private void Start() {
        playerCoord = new List<Vector3Int>();
        playerTurnsTaken = new List<bool>();

        foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
            playerCoord.Add(playerInfo.Key);
            playerTurnsTaken.Add(false);
        }
    }

    void Update() {
        if(playerTurn){
            
        }
        else {
            //FindObjectOfType<InputManager>().gameObject.SetActive(false);
        }

        bool allTurntaken = true;
        foreach(var turn in playerTurnsTaken) {
            if(!turn) {
                allTurntaken = false;
            }
        }

        if(allTurntaken) {
            allTurntaken = false;
            playerTurn = false;

            foreach(var turn in playerTurnsTaken) {
                if(!turn) {
                    playerTurn = false;
                }
            }

            FindAnyObjectByType<EnemyAi>().transform.GetComponent<EnemyAi>().enemyTurn();
        }
    }

    public void EnemyturnTaken() {
        playerTurn = true;
        FindObjectOfType<InputManager>().gameObject.SetActive(true);

        playerCoord = new List<Vector3Int>();
        playerTurnsTaken = new List<bool>();

        foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players) {
            playerCoord.Add(playerInfo.Key);
            playerTurnsTaken.Add(false);
        }

        Debug.LogWarning("Player Turn");
    }

    public void playerTookTurn(Vector3Int playerPos) {
        int index = playerCoord.IndexOf(playerPos);

        if(index == -1) {
            Debug.LogWarning("TurnManager - playerPos not found");
            return;
        }

        playerTurnsTaken[index] = true;
    }
}
