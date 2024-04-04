using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterLoader : MonoBehaviour {
    public SpawnWave playerSpawns, enemySpawns;
    public List<Stats> enemyStats;
    public List<Stats> tutorialPlayers;
    //public Stats swordsman, archer, healer, illusionist;

    private void Start() {
        if(SceneSwapper.currentScene == "Tutorial") {
            //GlobalVars.players.Add(new Vector3Int(6, 1, -7), swordsman);
            //GlobalVars.players.Add(new Vector3Int(7, 1, -8), swordsman);
            //GlobalVars.players.Add(new Vector3Int(8, 1, -9), swordsman);
            //GlobalVars.players.Add(new Vector3Int(0, 1, -10), swordsman);

            for (int i = 0; i < tutorialPlayers.Count; i++)
            {
                Tuple<string, Vector3Int> playerSpawnInfo = playerSpawns.spawns[i];
                Stats playerCharacter = GlobalVars.choosenPlayers[i];
                SpawnPlayer(playerSpawnInfo.Item2, playerCharacter.Copy());
            }

        }
        else
        {
            for(int i = 0; i < playerSpawns.spawns.Count; i++) {
                Tuple<string , Vector3Int> playerSpawnInfo = playerSpawns.spawns[i];
                //Stats playerCharacter = GlobalVars.choosenPlayers[i];
                //SpawnPlayer(playerSpawnInfo.Item2 , playerCharacter.Copy());
            }

            for(int i = 0; i < enemySpawns.spawns.Count; i++) {
                Tuple<string , Vector3Int> enemySpawnInfo = enemySpawns.spawns[i];
                Stats enemy = enemyStats.Find(x => x.charName == enemySpawnInfo.Item1);
                //Debug.Log(enemySpawnInfo.Item1);
                SpawnEnemy(enemySpawnInfo.Item2, enemy.Copy());
            }

            foreach(Stats stats in enemyStats) {
                GlobalVars.enemyStats.Add(stats);
            }
        }
    }

    public static void SpawnPlayer(Vector3Int spawnLoc, Stats playerStats) {
        GameObject currentTileObj = GlobalVars.hexagonTile[spawnLoc];
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = playerStats.sprite;

        playerStats.curHealth = playerStats.maxHealth;
        GlobalVars.players.Add(spawnLoc , playerStats);
    }

    public static void SpawnEnemy(Vector3Int spawnLoc , Stats stats) {
        Hex hex = new Hex(spawnLoc); //If spawnLoc is invalid, will send warning

        //Debug.Log("SpawnLoc: " + spawnLoc);
        GameObject enemyTileObj = GlobalVars.hexagonTile[spawnLoc];
        enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = stats.sprite;
        //Debug.Log("2");
        stats.curHealth = stats.maxHealth;
        //Debug.Log("2.5");
        GlobalVars.enemies.Add(spawnLoc , stats);
        //Debug.Log("3");
    }
}
