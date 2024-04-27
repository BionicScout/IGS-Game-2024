using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
    List<Vector3Int> playerCoords;
    List<int> playerMovement;
    List<bool> playerAction;
    List<bool> playerPotion;
    List<bool> playerTurnComplete;
    public bool allPlayerTurnsUsed;

    public  GameObject hitParticles;
    Vector3 worldSpacePos;

    public Queue<Command> commandQueue;

    public GameObject PlayerMenu;
    public GameObject WinMenu, LoseMenu;
    public InputManager InputManager;
    public DialogueTrigger DiaTrigger;

    int turn = 0;
    public Queue<SpawnWave> spawnQueue;
    SpawnWave nextWave;

    public string nextLevel;
    bool endState = false;

    private void Start() {
        playerCoords = new List<Vector3Int>();
        playerMovement = new List<int>(); //Movement speed left
        playerPotion = new List<bool>();
        playerAction = new List<bool>(); //False - Action not use, True - Action Used
        playerTurnComplete = new List<bool>(); //False - Player can move or do an action, True - Action Used and Moved Used
        worldSpacePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        allPlayerTurnsUsed = false;

        foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players) {
            playerCoords.Add(playerInfo.Key);
            playerMovement.Add(playerInfo.Value.move);
            playerPotion.Add(false);
            playerAction.Add(false);
            playerTurnComplete.Add(false);
        }

        commandQueue = new Queue<Command>();

        if(SceneSwapper.currentScene != "Tutorial")
        {
            nextWave = GlobalVars.spawnWaves.Dequeue();

        }
    }

    void Update() {
        if(SceneSwapper.currentScene != "Tutorial")
        {
            if (commandQueue.Count != 0)
            {
                enemyCommand(commandQueue.Dequeue());
            }

            //Win
            if (GlobalVars.enemies.Count == 0 && !endState) 
            {
                DialogueManager.GetInstance().EnterDialogueMode(DiaTrigger.outro);
                PlayerMenu.SetActive(false);
                AudioManager.instance.PlayFromSoundtrack("Win Level");
            }

            //Lose
            if(GlobalVars.players.Count == 0 && !endState) {
                PlayerMenu.SetActive(false);
                LoseMenuOpen();
                AudioManager.instance.PlayFromSoundtrack("Death-Screen");
            }
            if(GlobalVars.L1_houseTiles.Count == 0 && SceneSwapper.currentScene == "Level 1" && !endState) {
                PlayerMenu.SetActive(false);
                LoseMenu.SetActive(true);
                AudioManager.instance.PlayFromSoundtrack("Death-Screen");
            }

            if(Input.GetKeyDown(KeyCode.F)) {
                if(nextLevel == "FINAL LEVEL") {
                    SceneSwapper.A_LoadScene("MainMenu");
                }
                else {
                    SceneSwapper.holdLoadingScene = nextLevel;
                    SceneSwapper.A_LoadScene("LevelingUp");
                }
            }

        }
    }


    /*********************************
        Update Player Turn Stats
    *********************************/

    public void Player_Move(Vector3Int playerPos, int distance, Vector3Int newPos) {
        int playerIndex = playerCoords.FindIndex(a => a == playerPos);

        playerCoords[playerIndex] = newPos;

        Debug.Log("Distance: " + distance);
        playerMovement[playerIndex] = playerMovement[playerIndex] - distance;
        Debug.Log("Movement Left: " + playerMovement[playerIndex]);
    }

    public void Player_PotionAction(Vector3Int playerPos) {
        int playerIndex = playerCoords.FindIndex(a => a == playerPos);

        playerPotion[playerIndex] = true;
        playerMovement[playerIndex] = 0;
    }

    public void Player_HardAction(Vector3Int playerPos) {
        int playerIndex = playerCoords.FindIndex(a => a == playerPos);

        playerMovement[playerIndex] = 0;
        playerAction[playerIndex] = true;
        playerPotion[playerIndex] = true;
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
        //Debug.Log("PLAYER TURN HAS BEEN COMPLETED");
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
            playerPotion[i] = (false);
            playerTurnComplete[i] = (false);
        }
    }

    public void EndTurn() {
        if(SceneSwapper.currentScene == "Tutorial") { return; }
        UpdateActiveMenu();
        turn++;
        StartCoroutine(ExecuteEnemyTurn());
        InputManager.TakePoison();
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

    public bool getPotionUse(Vector3Int playerPos) {
        return playerPotion[playerCoords.FindIndex(a => a == playerPos)];
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
        int ogDodge = enemystats.dodge;

        if (command.moveSpace != Vector3Int.one && command.startSpace != command.moveSpace)
        {
            Movement.moveEnemy(command.startSpace, command.moveSpace);
        }
        else
        {
            InputManager.TakePoison();
        }

        //Instantiate(hitParticles, worldSpacePos , Quaternion.identity);

        //Enemy attack
        if (command.attackTile != Vector3Int.one) {
            Debug.Log("TURN MANAGER: " + command.attackTile);

            //Debug.Log("PLayer Health After: " + GlobalVars.players[command.attackTile].curHealth);

            if (InputManager.InSmoke())
            {
                enemystats.dodge = InputManager.smokeDodge;
                if(InputManager.RollDodge() > enemystats.dodge)
                {
                    Stats stats = GlobalVars.players[command.attackTile];
                    stats.curHealth -= stats.Damage(enemystats.power);
                    Instantiate(hitParticles, worldSpacePos, Quaternion.identity);
                    GlobalVars.players[command.attackTile] = stats;

                    PlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;

                    // Debug.Log("PLayer Health After: " + GlobalVars.players[command.attackTile].curHealth);

                    AudioManager.instance.Play("Player-Hurt");


                    //Player Dies
                    if(stats.curHealth <= 0) {
                        //Remove from this script
                        int playerIndex = playerCoords.FindIndex(x => x == command.attackTile);
                        playerCoords.RemoveAt(playerIndex);
                        playerMovement.RemoveAt(playerIndex);
                        playerAction.RemoveAt(playerIndex);
                        playerTurnComplete.RemoveAt(playerIndex);

                        //Remove From other scripts and scene
                        GameObject playerTileObj = GlobalVars.hexagonTile[command.attackTile];
                        playerTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                        //GlobalVars.deadPlayers.Add(GlobalVars.players[command.attackTile]);
                        GlobalVars.players.Remove(command.attackTile);
                        //death audio
                        //AudioManager.instance.Play("Player-Hurt");
                    }
                }
                enemystats.dodge = ogDodge;
            }
            else if(InputManager.RollDodge() > enemystats.dodge)
            {
                Stats stats = GlobalVars.players[command.attackTile];
                stats.curHealth -= stats.Damage(enemystats.power);
                Instantiate(hitParticles, worldSpacePos, Quaternion.identity);
                GlobalVars.players[command.attackTile] = stats;

                PlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;

                // Debug.Log("PLayer Health After: " + GlobalVars.players[command.attackTile].curHealth);

                AudioManager.instance.Play("Player-Hurt");


                //Player Dies
                if (stats.curHealth <= 0)
                {
                    //Remove from this script
                    int playerIndex = playerCoords.FindIndex(x => x == command.attackTile);
                    playerCoords.RemoveAt(playerIndex);
                    playerMovement.RemoveAt(playerIndex);
                    playerAction.RemoveAt(playerIndex);
                    playerTurnComplete.RemoveAt(playerIndex);

                    //Remove From other scripts and scene
                    GameObject playerTileObj = GlobalVars.hexagonTile[command.attackTile];
                    playerTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                    GlobalVars.players.Remove(command.attackTile);
                    //death audio
                    AudioManager.instance.Play("Player-Hurt");
                }
            }
        }

        //Attack House
        if(command.houseAttackTile != Vector3Int.one) {
            //Debug.Log("ATTACKED HOUSE - " +  command.houseAttackTile);
            TileScriptableObjects scriptableObject = GlobalVars.hexagonTileRefrence[command.houseAttackTile];        ;

            GameObject houseObj = GlobalVars.hexagonTile[command.houseAttackTile];

            houseObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = scriptableObject.objToChange.sprite;
            GlobalVars.hexagonTileRefrence[command.houseAttackTile] = scriptableObject.objToChange;

            GlobalVars.L1_houseTiles.Remove(command.houseAttackTile);
        }
    }

    public void SpawnEnemies() {
        turn++;

        if(GlobalVars.spawnWaves.Count == 1)
            GlobalVars.spawnWaves.Enqueue(new SpawnWave());

        SpawnWave currentWave = nextWave;
        nextWave = GlobalVars.spawnWaves.Dequeue();

        //Debug.Log("--- Wave " + turn + " ---");

        List<Tuple<string , Vector3Int>> nextWaveSpawns = new List<Tuple<string , Vector3Int>>();

        foreach(var enemySpawn in currentWave.spawns) {
            //Debug.Log("Maybe Spawn");
            // If unit is occupying space, spawn next round
            if(GlobalVars.players.ContainsKey(enemySpawn.Item2) || GlobalVars.enemies.ContainsKey(enemySpawn.Item2)) {
                nextWaveSpawns.Add(enemySpawn);
                //Debug.Log("Hold Spawn");
                continue;
            }

            //Debug.Log("Spawn");
            Stats enemy = GlobalVars.enemyStats.Find(x => x.charName == enemySpawn.Item1);
            //Debug.Log("SpawnLoc: " + enemySpawn.Item2);
            CharacterLoader.SpawnEnemy(enemySpawn.Item2 , enemy.Copy());
            //Debug.Log("After Spawn");

        }

        //Debug.Log("Exit Loop");

        // Add remaining spawns to the next wave
        nextWave.spawns.AddRange(nextWaveSpawns);
    }


    public void startPlayerTurn() {
        SpawnEnemies();
        //Debug.Log("Start Player Turn");
        ResetVals();
        //Debug.Log("Rest Vals");
        UpdateActiveMenu();
        //Debug.Log("Menu Back");
    }

    public void UpdateActiveMenu() {
        PlayerMenu.SetActive(!PlayerMenu.activeSelf);
    }
    public void LoseMenuOpen()
    {
        LoseMenu.SetActive(true);
        Transform menu = LoseMenu.transform.GetChild(0);
        Transform selectImage = menu.transform.GetChild(0).GetChild(0);
        int i = 0;
        foreach (KeyValuePair<Vector3Int, Stats> player in GlobalVars.players)
        {
            Image image = menu.GetChild(i).GetChild(0).GetComponent<Image>();
            image.sprite = player.Value.squareSprite;
            i++;
        }
    }
}