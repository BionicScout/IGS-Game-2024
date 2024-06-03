using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerAction : MonoBehaviour {

    CombatUI combatUI;
    TurnManager turnManager;
    TileIndicators tileIndicators;
    UnitUtilites unitUtilites;
    InputManager inputManager;

    [SerializeField]
    GameObject hitParticles;


    int poisonDamage = 0, smokeDodge = 0;

    private void Start() {
        combatUI = FindAnyObjectByType<CombatUI>();
        turnManager = FindAnyObjectByType<TurnManager>();
        tileIndicators = FindAnyObjectByType<TileIndicators>();
        unitUtilites = FindAnyObjectByType<UnitUtilites>();
        inputManager = FindAnyObjectByType<InputManager>();


        //Get Smoke Dodge and Posion Damage
        foreach(KeyValuePair<Vector3Int , Stats> player in GlobalVars.players) {
            //Characters specific tings that need to be stored
            if(GlobalVars.players[player.Key].charType == "Alchemist") {
                poisonDamage = GlobalVars.players[player.Key].power;
            }
            if(GlobalVars.players[player.Key].charType == "Illusionist") {
                smokeDodge = GlobalVars.players[player.Key].power;
            }
        }
    }

    public void Shoot(Vector3Int enemyCoord, Vector3Int playerCoord) {
        //Check if condtions are met
        if(turnManager.getActionUse(playerCoord)) { Debug.Log("No More Action Points"); return; }
        if(!GlobalVars.enemies.ContainsKey(enemyCoord)) { Debug.Log("No Enemy on Click"); return; }

        Stats playerStats = GlobalVars.players[playerCoord];
        if(Pathfinding.PathBetweenPoints(enemyCoord, playerCoord , false).Count > playerStats.attackRange + 1) { Debug.Log("Target Not in Range"); return; }

        tileIndicators.ClearIndicators(playerCoord);
        tileIndicators.AttackIndicators(false , playerCoord , playerStats.attackRange);


        //Get enemy's Stats
        Stats enemyStats = GlobalVars.enemies[enemyCoord];
        GameObject enemyTileObj = GlobalVars.hexagonTile[enemyCoord];

        int dodgeChange = enemyStats.dodge;
        if(unitUtilites.InSmoke())
            dodgeChange = smokeDodge;

        //Check if enemy Dodges
        if(Random.Range(1 , 100) > dodgeChange) {
            //Damage Enemy
            enemyStats.Damage(playerStats.power);
            Debug.Log(combatUI);
            combatUI.Enemy_UpdateHealth(enemyCoord);

            //attack audio
            AudioManager.instance.Play("Player Attack");
            //hit particles
            Instantiate(hitParticles, GlobalVars.hexagonTile[enemyCoord].transform.position, Quaternion.identity);

            GameObject newTileObj = GlobalVars.hexagonTile[enemyCoord];
            newTileObj.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = enemyStats.curHealth / enemyStats.maxHealth;

            //enemy death
            if(enemyStats.curHealth <= 0) {
                GlobalVars.enemies.Remove(enemyCoord);
                enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                //RollItems();
                //death audio
                newTileObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                //AudioManager.instance.Play("Death-Sound");
            }
        }
        else
            Debug.Log("Player Dodged");

        //Update Unit Info
        turnManager.Player_HardAction(playerCoord);
        GlobalVars.players.Remove(enemyCoord);

        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerStats.power;
        GlobalVars.players[playerCoord].defense = playerStats.defense;
    }


    public void Poison(Vector3Int clickedCoord , Vector3Int playerCoord) {
        //Check if condtions are met
        if(turnManager.getActionUse(playerCoord)) { Debug.Log("No More Action Points"); return; }

        Stats playerStats = GlobalVars.players[playerCoord];


        tileIndicators.ClearIndicators(playerCoord);
        tileIndicators.AttackIndicators(false , playerCoord , playerStats.attackRange);



        tileIndicators.ClearIndicators(playerCoord);
        turnManager.Player_HardAction(playerCoord);

        if(GlobalVars.players[playerCoord].charLevel <= 2) {
            if(Vector3Int.Distance(clickedCoord , playerCoord) <= playerStats.attackRange + 1) {
                tileIndicators.AttackIndicators(false , playerCoord , playerStats.attackRange);
                //adds selected tile to a dictionary and sets an indicaotor active
                GlobalVars.poisonTiles.Add(clickedCoord , 2);
                GlobalVars.hexagonTile[clickedCoord].transform.GetChild(4).gameObject.SetActive(true);

                Pathfinding.AllPossibleTiles(clickedCoord , playerStats.attackRange , false);

                //Update player coord
                GlobalVars.players.Remove(clickedCoord);
            }
        }
        else if(GlobalVars.players[playerCoord].charLevel == 3) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1 , false)) {
                //adds selected tiles to a dictionary and sets an indicaotors active
                Vector3Int t = temp.Item1;
                GlobalVars.poisonTiles.Add(t , 2);
                foreach(Tuple<Vector3Int , int> coord in Pathfinding.AllPossibleTiles(clickedCoord , 1 , false)) {
                    Vector3Int c = coord.Item1;
                    GlobalVars.poisonTiles.Add(t , 2);
                    GlobalVars.hexagonTile[c].transform.GetChild(4).gameObject.SetActive(true);
                }
            }

            Pathfinding.AllPossibleTiles(clickedCoord , playerStats.attackRange , false);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerStats.power;
        GlobalVars.players[playerCoord].defense = playerStats.defense;
    }

    public void SmokeBomb(Vector3Int clickedCoord , Vector3Int playerCoord) {
        tileIndicators.ClearIndicators(playerCoord);
        turnManager.Player_HardAction(playerCoord);

        Stats playerStats = GlobalVars.enemies[playerCoord];

        if(Vector3Int.Distance(clickedCoord , playerCoord) <= playerStats.attackRange + 1) {
            tileIndicators.AttackIndicators(false , playerCoord , playerStats.attackRange);

            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(clickedCoord , 1 , false)) {
                //adds selected tiles to a dictionary and sets an indicaotors active
                Vector3Int t = temp.Item1;
                GlobalVars.smokeTiles.Add(t , 2);
                GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(true);
            }
            Pathfinding.AllPossibleTiles(clickedCoord , playerStats.attackRange , false);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
            AudioManager.instance.Play("Potion");
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerStats.power;
        GlobalVars.players[playerCoord].defense = playerStats.defense;
    }

    public void Heal(int healthBack, Vector3Int clickedCoord , Vector3Int playerCoord) {
        tileIndicators.ClearIndicators(playerCoord);
        if(turnManager.getActionUse(playerCoord)) {
            if(GlobalVars.players[clickedCoord].charLevel == 1) {
                if(GlobalVars.players.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= 2) {
                    //Get Player and current + future hex objs
                    Stats playerStats = GlobalVars.players[clickedCoord];
                    GameObject playerTileObj = GlobalVars.hexagonTile[clickedCoord];

                    //heal player
                    playerStats.Heal(healthBack);
                    combatUI.Player_UpdateHealth(clickedCoord);
                    Debug.Log("Player Healed!!");

                    //turns off indicator
                    tileIndicators.HealIndicators(false , playerCoord);

                    turnManager.Player_HardAction(playerCoord);

                    //Update player coord
                    GlobalVars.players.Remove(clickedCoord);

                    GameObject newTileObj = GlobalVars.hexagonTile[clickedCoord];
                    newTileObj.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = playerStats.curHealth / playerStats.maxHealth;
                }
            }
            else if(GlobalVars.players[clickedCoord].charLevel == 2) {
                foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1 , false)) {
                    //heals all players in a 1 tile range
                    Vector3Int t = temp.Item1;
                    GlobalVars.players[t].Heal(healthBack);
                    Debug.Log("All players in 1 range are healed");
                }

                //turns off indicator
                tileIndicators.HealIndicators(false , playerCoord);

                turnManager.Player_HardAction(playerCoord);

                //Update player coord
                GlobalVars.players.Remove(clickedCoord);
            }
            //resets a players power and defense incase they used an item

            Stats playerStats2 = GlobalVars.enemies[playerCoord];
            GlobalVars.players[playerCoord].power = playerStats2.power;
            GlobalVars.players[playerCoord].defense = playerStats2.defense;
        }
    }

    public void Move(Vector3Int clickedCoord , Vector3Int playerCoord) {
        tileIndicators.ClearIndicators(playerCoord);

        Debug.Log(Pathfinding.PathBetweenPoints(clickedCoord , playerCoord , false).Count);

        int moveRange = turnManager.getMovementLeft(playerCoord);
        List<Tuple<Vector3Int , int>> possibles = Pathfinding.AllPossibleTiles(clickedCoord , moveRange , true);


        foreach(Tuple<Vector3Int , int> temp in possibles) {
            Debug.Log("--------------------");
            int dist = Pathfinding.PathBetweenPoints(temp.Item1 , playerCoord , false).Count - 1;

            foreach(Vector3Int pos in Pathfinding.PathBetweenPoints(temp.Item1 , playerCoord , true)) {
                Debug.Log(pos);
            }

            Debug.Log(dist);
            //Debug.Log();

            if(temp.Item1 == clickedCoord && dist <= moveRange + 1) {
                Debug.Log("Hi");
                Movement.movePlayer(playerCoord , clickedCoord);
                //moveRadioWheel();
                tileIndicators.MoveIndicators(false , playerCoord);
                inputManager.TakePoison();

                AudioManager.instance.Play("Move");

                turnManager.Player_Move(playerCoord , dist , clickedCoord);
                playerCoord = clickedCoord;
                break;
            }
        }
    }
    public void Interact(Vector3Int clickedCoord , Vector3Int playerCoord) {
        //Debug.Log(turnManager.getActionUse(playerCoord));
        if(!turnManager.getActionUse(playerCoord)) {
            Debug.Log("INTERACT");

            if(GlobalVars.L2_trees.Contains(clickedCoord)) {
                //Convert Tree to Laying Down, changing specific tiles sprites
                TileScriptableObjects mainTileInfo = GlobalVars.hexagonTileRefrence[clickedCoord];

                foreach(Vector3Int offset in GlobalVars.hexagonTileRefrence[clickedCoord].tileChanges) {
                    Vector3Int newCoord = clickedCoord + offset;

                    GameObject tileObj = GlobalVars.hexagonTile[newCoord];
                    tileObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mainTileInfo.objToChange.sprite;
                    GlobalVars.hexagonTileRefrence[newCoord] = mainTileInfo.objToChange;
                }

                GlobalVars.L2_trees.Remove(clickedCoord);

                //Turn all enemies to generic ai
                foreach(KeyValuePair<Vector3Int , Stats> enemy in GlobalVars.enemies) {
                    enemy.Value.charType = "General";
                }
            }

            if(GlobalVars.L3_trees.Contains(clickedCoord)) {
                //Convert Tree to Laying Down, changing specific tiles sprites
                TileScriptableObjects mainTileInfo = GlobalVars.hexagonTileRefrence[clickedCoord];

                foreach(Vector3Int offset in GlobalVars.hexagonTileRefrence[clickedCoord].tileChanges) {
                    Vector3Int newCoord = clickedCoord + offset;

                    GameObject tileObj = GlobalVars.hexagonTile[newCoord];
                    tileObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mainTileInfo.objToChange.sprite;
                    GlobalVars.hexagonTileRefrence[newCoord] = mainTileInfo.objToChange;
                }

                GlobalVars.L3_trees.Remove(clickedCoord);

                //Turn all enemies to generic ai
                foreach(KeyValuePair<Vector3Int , Stats> enemy in GlobalVars.enemies) {
                    enemy.Value.charType = "General";
                }
            }

            if(GlobalVars.L4_Buttons.Contains(clickedCoord)) {
                //Changes buttons tile to button pressed
                TileScriptableObjects mainTileInfo = GlobalVars.hexagonTileRefrence[clickedCoord];

                foreach(Vector3Int offset in GlobalVars.hexagonTileRefrence[clickedCoord].tileChanges) {
                    Vector3Int newCoord = clickedCoord + offset;

                    GameObject tileObj = GlobalVars.hexagonTile[newCoord];
                    tileObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mainTileInfo.objToChange.sprite;
                    GlobalVars.hexagonTileRefrence[newCoord] = mainTileInfo.objToChange;
                }

                GlobalVars.L4_Buttons.Remove(clickedCoord);

                if(GlobalVars.L4_Buttons.Count == 0) {
                    foreach(KeyValuePair<Vector3Int , TileScriptableObjects> tile in GlobalVars.hexagonTileRefrence) {
                        if(tile.Value.sprite.name == "Cliff_Low_comp") {
                            GameObject tileObj = GlobalVars.hexagonTile[tile.Key];
                            tileObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tile.Value.objToChange.sprite;
                            GlobalVars.hexagonTileRefrence[tile.Key] = tile.Value.objToChange;
                        }
                    }
                }
            }

            turnManager.Player_HardAction(playerCoord);
        }
    }

}
