using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
    List<Vector3Int> playerCoords;
    List<int> playerMovement;
    List<bool> playerAction;
    List<bool> playerTurnComplete;
    bool allPlayerTurnsUsed;

    public Queue<Command> commandQueue;

    public GameObject PlayerMenu;
    public GameObject WinMenu, LoseMenu;

    int turn = 0;
    public Queue<SpawnWave> spawnQueue;
    SpawnWave nextWave;

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

        nextWave = GlobalVars.spawnWaves.Dequeue();
    }

    void Update() {
        if(commandQueue.Count != 0) {
            enemyCommand(commandQueue.Dequeue());
        }

        if(GlobalVars.enemies.Count == 0) {
            PlayerMenu.SetActive(false);
            LoseMenu.SetActive(true);

            
        }

        if(GlobalVars.enemies.Count == 0) {
            PlayerMenu.SetActive(false);
            WinMenu.SetActive(true);
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
        turn++;
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
        //Move
        Stats enemystats = GlobalVars.enemies[command.startSpace];
        Movement.moveEnemy(command.startSpace, command.moveSpace);

        //Instantiate(hitParticles, worldSpacePos , Quaternion.identity);

        //Enemy attack
        if(command.attackTile != Vector3Int.one) {
            Debug.Log(command.attackTile);

            Debug.Log("PLayer Health After: " + GlobalVars.players[command.attackTile].curHealth);

            Stats stats = GlobalVars.players[command.attackTile];
            stats.curHealth -= stats.Damage(enemystats.power);
            GlobalVars.players[command.attackTile] = stats;

            PlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;

            Debug.Log("PLayer Health After: " + GlobalVars.players[command.attackTile].curHealth);


            //Player Dies
            if(stats.curHealth <= 0) {
                GameObject playerTileObj = GlobalVars.hexagonTile[command.attackTile];
                playerTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                GlobalVars.players.Remove(command.attackTile);
                //death audio
                AudioManager.instance.Play("Player-Hurt");
            }
        }
    }

    public void SpawnEnemies() {
        turn++;

        if(GlobalVars.spawnWaves.Count == 1)
            GlobalVars.spawnWaves.Enqueue(new SpawnWave());

        SpawnWave currentWave = nextWave;
        nextWave = GlobalVars.spawnWaves.Dequeue();

        Debug.Log("--- Wave " + turn + " ---");

        List<Tuple<string , Vector3Int>> nextWaveSpawns = new List<Tuple<string , Vector3Int>>();

        foreach(var enemySpawn in currentWave.spawns) {
            Debug.Log("Maybe Spawn");
            // If unit is occupying space, spawn next round
            //if(GlobalVars.players.ContainsKey(enemySpawn.Item2) || GlobalVars.enemies.ContainsKey(enemySpawn.Item2)) {
            //    nextWaveSpawns.Add(enemySpawn);
            //    Debug.Log("Hold Spawn");
            //    continue;
            //}

            Debug.Log("Spawn");
            Stats enemy = GlobalVars.enemyStats.Find(x => x.charName == enemySpawn.Item1);
            CharacterLoader.SpawnEnemy(enemySpawn.Item2 , enemy.Copy());
        }

        Debug.Log("Exit Loop");

        // Add remaining spawns to the next wave
        nextWave.spawns.AddRange(nextWaveSpawns);
    }


    public void startPlayerTurn() {
        SpawnEnemies();
        Debug.Log("Start Player Turn");
        ResetVals();
        UpdateActiveMenu();
    }

    public void UpdateActiveMenu() {
        PlayerMenu.SetActive(!PlayerMenu.activeSelf);
    }
}