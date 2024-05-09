using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerAction : MonoBehaviour {

    CombatUI CombatUI;
    TurnManager turnManager;
    TileIndicators tileIndicators;
    UnitUtilites unitUtilites;

    [SerializeField]
    GameObject hitParticles;


    int poisonDamage = 0, smokeDodge = 0;

    private void Start() {
        CombatUI = FindAnyObjectByType<CombatUI>();
        turnManager = FindAnyObjectByType<TurnManager>();
        tileIndicators = FindAnyObjectByType<TileIndicators>();
        unitUtilites = FindAnyObjectByType<UnitUtilites>();


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
            Debug.Log(CombatUI);
            CombatUI.Enemy_UpdateHealth(enemyCoord);

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
}
