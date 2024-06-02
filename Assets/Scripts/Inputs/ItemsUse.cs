using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemsUse : MonoBehaviour
{

    CombatUI combatUI;
    TurnManager turnManager;
    TileIndicators tileIndicators;
    UnitUtilites unitUtilites;
    InputManager inputManager;

    public Items items;

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

    /*********************************
            Item Functions
    *********************************/
    public void SingleHeal() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        //heals the character
        if(items.singleHealAMT != 0) {
            Debug.Log("Player was Healed and item was used");
            if(GlobalVars.players[playerCoord].curHealth + 7 >= GlobalVars.players[playerCoord].maxHealth) {
                GlobalVars.players[playerCoord].curHealth = GlobalVars.players[playerCoord].maxHealth;
                combatUI.UpdateHealth(GlobalVars.players[playerCoord].curHealth);
            }
            GlobalVars.players[playerCoord].curHealth += 7;
            combatUI.UpdateHealth(GlobalVars.players[playerCoord].curHealth);
            AudioManager.instance.Play("Potion");
            combatUI.Player_UpdateHealth(playerCoord);
            items.singleHealAMT--;
            turnManager.Player_PotionAction(playerCoord);
        }

        combatUI.ItemMenu();
    }
    //adds one to the characters power 
    public void PowerBuff() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        items.powerBuffUsed = true;
        if(items.powerBuffAMT != 0) {
            GlobalVars.players[playerCoord].power++;
            //end it after a turn?
            AudioManager.instance.Play("Potion");
            items.powerBuffAMT--;
            turnManager.Player_PotionAction(playerCoord);
        }

        combatUI.ItemMenu();
    }
    //adds one to te character defense
    public void DefenseBuff() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        items.defenseBuffUsed = true;
        if(items.defenseBuffAMT != 0) {
            GlobalVars.players[playerCoord].defense++;
            AudioManager.instance.Play("Potion");
            items.defenseBuffAMT--;
            turnManager.Player_PotionAction(playerCoord);
        }

        combatUI.ItemMenu();
    }
    //heals all characters
    public void HealScroll() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        if(items.healScrollAMT != 0) {
            foreach(var health in GlobalVars.players.Values) {
                //Debug.Log("all Player was healed");
                if(GlobalVars.players[playerCoord].curHealth + 4 >= GlobalVars.players[playerCoord].maxHealth) {
                    GlobalVars.players[playerCoord].curHealth = GlobalVars.players[playerCoord].maxHealth;
                    combatUI.UpdateHealth(GlobalVars.players[playerCoord].curHealth);
                }
                GlobalVars.players[playerCoord].curHealth += 4;
                combatUI.UpdateHealth(GlobalVars.players[playerCoord].curHealth);
                AudioManager.instance.Play("Potion");
                combatUI.Player_UpdateHealth(playerCoord);
            }

            turnManager.Player_PotionAction(playerCoord);
            items.healScrollAMT--;
        }

        combatUI.ItemMenu();
    }

    /*********************************
        Other
    *********************************/

    public float RollItems() {
        Vector3Int clickedCoord = inputManager.getClickedCoord();

        float itemChance = Random.Range(1 , 100);
        if(itemChance >= 50) {
            Debug.Log("Item was dropped");
            float whatItem = Random.Range(1 , 100);
            if(itemChance > 0 && itemChance <= GlobalVars.enemies[clickedCoord].singleHealDrop) {
                //drops single heal
                Debug.Log("Single heal was dropped");
            }
            else if(itemChance > 51 && itemChance <= GlobalVars.enemies[clickedCoord].powerBuffDrop) {
                //drops power buff
                Debug.Log("Power buff was dropped");
            }
            else if(itemChance > 66 && itemChance <= GlobalVars.enemies[clickedCoord].defBuffDrop) {
                //drops defense buff
                Debug.Log("defense buff was dropped");
            }
            else if(itemChance > 81 && itemChance <= GlobalVars.enemies[clickedCoord].reviveDrop) {
                //drops revive
                Debug.Log("revive was dropped");
            }
            else if(itemChance > 91 && itemChance <= GlobalVars.enemies[clickedCoord].healScrollDrop) {
                //drops healing scroll
                Debug.Log("healing scroll was dropped");
            }
        }
        return 0;
    }
}
