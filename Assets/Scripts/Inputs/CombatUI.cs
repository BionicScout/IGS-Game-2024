using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {
    [Header("Menus")]
    public GameObject selectedPlayerMenu;
    public GameObject radioMenu;
    public GameObject statsMenu;
    public GameObject itemMenu;

    public Items items;

    [Header("Debug")]
    public bool buttonDebug = false;

    TileIndicators tileIndicators;
    TurnManager turnManager;
    InputManager inputManager;

    private void Start() {
        tileIndicators = FindAnyObjectByType<TileIndicators>();
        inputManager = FindAnyObjectByType<InputManager>();
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    public void Player_UpdateHealth(Vector3Int playerCoord) {
        Stats playerStats = GlobalVars.enemies[playerCoord];
        GameObject currentTileObj = GlobalVars.hexagonTile[playerCoord];

        currentTileObj.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = playerStats.curHealth / playerStats.maxHealth;
    }

    public void Enemy_UpdateHealth(Vector3Int enemyCoord) {
        Stats enemyStats = GlobalVars.enemies[enemyCoord];
        GameObject currentTileObj = GlobalVars.hexagonTile[enemyCoord];

        currentTileObj.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = enemyStats.curHealth / enemyStats.maxHealth;
    }

    /*********************************
        Update Input Mode
    *********************************/
    public void SetAttack() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        tileIndicators.ClearIndicators(playerCoord);
        Stats playerStats = GlobalVars.players[playerCoord];
        tileIndicators.AttackIndicators(true , playerCoord, playerStats.attackRange);
        inputManager.inputMode = InputManager.modes.attack;
        inputManager.clickedUI = true;

        if(buttonDebug) { Debug.Log("CombatUI - Attack Button Pressed"); }
    }
    public void SetMove() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        if(turnManager.getMovementLeft(playerCoord) <= 0) {
            return;
        }

        tileIndicators.ClearIndicators(playerCoord);
        tileIndicators.MoveIndicators(true , playerCoord);
        inputManager.inputMode = InputManager.modes.move;
        inputManager.clickedUI = true;

        if(buttonDebug) { Debug.Log("CombatUI - Move Button Pressed"); }
    }
    public void SetSmoke() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        tileIndicators.ClearIndicators(playerCoord);
        inputManager.inputMode = InputManager.modes.AOE;
        inputManager.clickedUI = true;

        if(buttonDebug) { Debug.Log("CombatUI - Smoke Button Pressed"); }
    }
    public void SetHeal() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        tileIndicators.ClearIndicators(playerCoord);
        inputManager.inputMode = InputManager.modes.heal;
        inputManager.clickedUI = true;

        if(buttonDebug) { Debug.Log("CombatUI - Heal Button Pressed"); }
    }
    public void SetInteract() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        tileIndicators.ClearIndicators(playerCoord);
        tileIndicators.InteractIndicators(playerCoord);
        inputManager.inputMode = InputManager.modes.interact;

        if(buttonDebug) { Debug.Log("CombatUI - Interact Button Pressed"); }
    }


    /*********************************
        Radio Wheel
    *********************************/
    public void moveRadioWheel() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        //Vector3 screenPosition = cam.WorldToScreenPoint(GlobalVars.hexagonTile[playerCoord].transform.position);
        //radioMenu.transform.position = screenPosition;
    }

    public void toggleRadioMenu() {
        radioMenu.SetActive(radioMenu.activeSelf);
    }

    /*********************************
        UI
    *********************************/
    public void UpdatePlayerMenu() {
        statsMenu.SetActive(false);

        Vector3Int playerCoord = inputManager.getPlayerCoord();
        Stats stats = GlobalVars.players[playerCoord];
        //updates sprites and health
        selectedPlayerMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = stats.squareSprite;
        //selectedPlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;
    }
    //sets a menu active and changes all the information
    public void StatsMenu() {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        statsMenu.SetActive(!statsMenu.activeSelf);

        Transform baseObj = statsMenu.transform.GetChild(0);

        Stats stats = GlobalVars.players[playerCoord];
        baseObj.GetChild(3).GetComponent<TMP_Text>().text = stats.charType.ToString();
        baseObj.GetChild(6).GetChild(1).GetComponent<TMP_Text>().text = stats.move.ToString();
        baseObj.GetChild(6).GetChild(0).GetComponent<TMP_Text>().text = stats.power.ToString();
        baseObj.GetChild(6).GetChild(3).GetComponent<TMP_Text>().text = stats.defense.ToString();
        baseObj.GetChild(6).GetChild(4).GetComponent<TMP_Text>().text = stats.maxHealth.ToString();
        baseObj.GetChild(6).GetChild(2).GetComponent<TMP_Text>().text = stats.attackRange.ToString();

        if(buttonDebug) { Debug.Log("CombatUI - Stats Menu set to " + statsMenu.activeSelf); }
    }
    //sets a menu active and changes all the information
    public void ItemMenu() {
        itemMenu.SetActive(!itemMenu.activeSelf);

        items.singleHealTxt.text = "x" + items.singleHealAMT.ToString();
        items.powerBuffTxt.text = "x" + items.powerBuffAMT.ToString();
        items.defenseBuffTxt.text = "x" + items.defenseBuffAMT.ToString();
        items.healScrollTxt.text = "x" + items.healScrollAMT.ToString();

        if(buttonDebug) { Debug.Log("CombatUI - Item Menu set to " + itemMenu.activeSelf); }
    }
    public void UpdateHealth(float healthOffset) {
        Vector3Int playerCoord = inputManager.getPlayerCoord();

        Stats stats = GlobalVars.players[playerCoord];
        stats.curHealth += healthOffset;
        GlobalVars.players[playerCoord] = stats;
        Player_UpdateHealth(playerCoord);

        selectedPlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;
    }

    /*********************************
        Banner
    *********************************/

}
