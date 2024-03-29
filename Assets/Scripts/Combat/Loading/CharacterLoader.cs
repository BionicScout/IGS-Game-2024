using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterLoader : MonoBehaviour {
    public SpawnWave playerSpawns, enemySpawns;
    public List<Stats> enemyStats;

    private void Start() {
        for(int i = 0; i < playerSpawns.spawns.Count; i++) {
            Tuple<string , Vector3Int> playerSpawnInfo = playerSpawns.spawns[i];
            Stats playerCharacter = GlobalVars.choosenPlayers[i];
            SpawnPlayer(playerSpawnInfo.Item2 , playerCharacter.Copy());
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





















    //[SerializeField]
    //public List<Stats> enemies;

    //private void Start() {
    //    //Player
    //    for(int i = 0; i < GlobalVars.choosenPlayers.Count; i++) {
    //        Vector3Int tile = GlobalVars.centerHex + (i * Hex.hex_directions[0]);
    //        GlobalVars.players.Add(tile , GlobalVars.choosenPlayers[i]);
    //        GameObject currentTileObj = GlobalVars.hexagonTile[tile];
    //        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = GlobalVars.choosenPlayers[i].sprite;

    //        GlobalVars.players[tile].curHealth = GlobalVars.players[tile].maxHealth;
    //    }

    //    //Random Offsets
    //    List<Vector3Int> offsets = new List<Vector3Int>();
    //    Vector3Int maxTile = GlobalVars.centerHex * 2;

    //    for(int i = 0; i < 10; i++) {
    //        Hex randHex = new Hex(Random.Range(0, maxTile.x), Random.Range(0 , maxTile.y));

    //        if(GlobalVars.hexagonTileRefrence[new Vector3Int(randHex.q, randHex.r, randHex.s)].isObstacle == false) {
    //            offsets.Add(new Vector3Int(randHex.q , randHex.r , randHex.s));
    //        }
    //    }

    //    //Enemey
    //    foreach(Vector3Int coord in offsets) {
    //        GlobalVars.enemies.Add(coord , enemies[0]);
    //        GameObject enemyTileObj = GlobalVars.hexagonTile[coord];
    //        enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = enemies[0].sprite;

    //        GlobalVars.enemies[coord].curHealth = GlobalVars.enemies[coord].maxHealth;
    //    }

    //    //Debug.Log("Enemies: " + GlobalVars.enemies);

    //}
}
