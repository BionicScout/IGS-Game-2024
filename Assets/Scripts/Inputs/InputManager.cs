using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;
using UnityEditor;
using System.Drawing;
using System.Linq;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;


public class InputManager : MonoBehaviour {
    enum modes {
        normal = 0,
        attack = 1,
        move = 2,
        AOE = 3,
        heal = 4,
        interact = 5
    }

    modes inputMode;
    public bool clickedUI = false;
    public Items items;

    [SerializeField]
    GameObject hitParticles;
    [SerializeField] 
    public Slider slider;

    [Header("Stats")]
    public int playerMove;
    public int playerPower;
    public int playerAttRange;
    public int playerDefense;
    public int playerMaxHealth;
    public int poisonDmg;
    public int smokeDodge; 

    static Vector3Int clickedCoord, playerCoord, enemyCoord, mouseCoord;
    Vector3 worldSpacePos;
    [Header("Menus")]
    public GameObject selectedPlayerMenu;
    public GameObject statsMenu;
    public GameObject itemMenu;
    [Header("Text")]
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI powerTxt;
    public TextMeshProUGUI defenseTxt;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI powerRangeTxt;
    public TextMeshProUGUI charTypeTxt;


    //HexObjInfo hexObjInfo;

    TurnManager turnManager;

    /*********************************
        Start and Update
    *********************************/
    void Start() {
        inputMode = modes.normal;
        selectedPlayerMenu.SetActive(true);
        statsMenu.SetActive(false);
        turnManager = FindAnyObjectByType<TurnManager>();

    }
    void Update() {
        if(clickedUI) {
            clickedUI = false;
            return;
        }

        GetPosition();
        //checked if the player lef clicked and gets the coord
        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x , mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D , Vector2.zero);
            foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(clickedCoord, 75, false))
            {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(5).gameObject.SetActive(false);
            }
            ClearIndicators();
            GlobalVars.hexagonTile[clickedCoord].transform.GetChild(5).gameObject.SetActive(true);

            worldSpacePos = mousePos;
            //if the player clicked on a collider the if their is a character on that tile
            if(hit.collider != null) {
                if (GlobalVars.players.ContainsKey(clickedCoord)) {
                    playerCoord = clickedCoord;
                    UpdatePlayerMenu();

                    //gets players stats annd stores them
                    playerMove = GlobalVars.players[clickedCoord].move;
                    playerPower = GlobalVars.players[clickedCoord].power;
                    playerAttRange = GlobalVars.players[clickedCoord].attackRange;
                    playerDefense = GlobalVars.players[clickedCoord].defense;
                    playerMaxHealth = GlobalVars.players[clickedCoord].maxHealth;

                    //Characters specific tings that need to be stored
                    if (GlobalVars.players[clickedCoord].charType == "Alchemist")
                    {
                        poisonDmg = GlobalVars.players[clickedCoord].power;
                    }
                    if (GlobalVars.players[clickedCoord].charType == "Illusionist")
                    {
                        smokeDodge = GlobalVars.players[clickedCoord].power;
                    }

                    //sets all indicators false when players are clicked
                    ClearIndicators();

                }
            }
            //triggers the specific functions from the buttons
            if(inputMode == modes.attack) {
                if (GlobalVars.players[playerCoord].charType == "Swordsman" || GlobalVars.players[playerCoord].charType == "Spearman" || GlobalVars.players[playerCoord].charType == "Paladin")
                {
                    Debug.Log("Wack function was called");
                    Wack(clickedCoord, 2);
                    inputMode = modes.normal;
                }
                else if (GlobalVars.players[playerCoord].charType == "Archer" || GlobalVars.players[playerCoord].charType == "Crossbowman")
                {
                    Shoot(clickedCoord, 2);
                    Debug.Log("Shoot function was called");
                    inputMode = modes.normal;
                }
                else if(GlobalVars.players[playerCoord].charType == "Alchemist")
                {
                    Poison();
                    inputMode = modes.normal;
                }
                else if (GlobalVars.players[clickedCoord].charType == "Cleric")
                {
                    Heal(GlobalVars.players[clickedCoord].power);
                    inputMode = modes.normal;
                }
                else if (GlobalVars.players[playerCoord].charType == "Illusionist")
                {
                    SmokeBomb();
                    inputMode = modes.normal;
                }
            }
            if(inputMode == modes.move) {
                Move();
                inputMode = modes.normal;
            }
            if (inputMode == modes.AOE)
            {
                SmokeBomb();
                inputMode = modes.normal;
            }
            if (inputMode == modes.heal)
            {
                Heal(playerPower);
                inputMode = modes.normal;
            }
            if(inputMode == modes.interact)
            {
                Interact();
                inputMode = modes.normal;
            }
        }

        //Debug stuff
        if(Input.GetKeyDown(KeyCode.G)) {
            UpdateHealth(-1);
        }
        if(Input.GetKeyDown(KeyCode.H)) {
            UpdateHealth(1);
        }
    }

    /*********************************
        Update Input Mode
    *********************************/
    public void SetAttack() {
        ClearIndicators();
        AttackIndicators(true);
        inputMode = modes.attack;
        clickedUI = true;
    }
    public void SetMove() {
        ClearIndicators();
        MoveIndicators(true);
        inputMode = modes.move;
        clickedUI = true;
    }
    public void SetSmoke() {
        ClearIndicators();
        inputMode = modes.AOE;
        clickedUI = true;
    }
    public void SetHeal() {
        ClearIndicators();
        inputMode = modes.heal;
        clickedUI = true;
    }
    public void SetInteract()
    {
        ClearIndicators();
        InteractIndicators();
        inputMode = modes.interact;
    }

    /*********************************
        Actions
    *********************************/
    public void Shoot(Vector3Int hexCoordOfEnemy , float damage) {
        Debug.Log("Shoot Function has started");
        ClearIndicators();
        int ogDodge = GlobalVars.players[playerCoord].dodge;

        if(GlobalVars.enemies.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= playerAttRange + 1) {
            Debug.Log("checked if enemy was in range");
            //Get Player and current + future hex objs
            Stats enemyStats = GlobalVars.enemies[clickedCoord];
            GameObject enemyTileObj = GlobalVars.hexagonTile[clickedCoord];

            if (InSmoke())
            {
                Debug.Log("Checked if player is in smoke");
                GlobalVars.players[clickedCoord].dodge = smokeDodge;
                if (RollDodge() > GlobalVars.players[clickedCoord].dodge)
                {
                    Debug.Log("Rolled and checked to see in the enemy dodged");
                    //deals damage
                    enemyStats.Damage(playerPower);
                    Debug.Log("Damage was delt");
                    Debug.Log("enemy health: " + enemyStats.curHealth);
                    //attack audio
                    AudioManager.instance.Play("Attack");
                    //hit particles
                    Instantiate(hitParticles, worldSpacePos, Quaternion.identity);

                    //enemy death
                    if (enemyStats.curHealth <= 0) {
                        RemoveEnmey(hexCoordOfEnemy);
                        enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                        //RollItems();
                        //death audio
                        AudioManager.instance.Play("Deah-Sound");
                    }
            }
            GlobalVars.players[clickedCoord].dodge = ogDodge;
        }
            else if (RollDodge() > GlobalVars.players[playerCoord].dodge)
            {
                Debug.Log("Rolled and checked to see in the enemy dodged");
                //deals damage
                Debug.Log("Damage was delt");
                enemyStats.Damage(playerPower);
                //attack audio
                AudioManager.instance.Play("Attack");
                //hit particles
                Instantiate(hitParticles, worldSpacePos, Quaternion.identity);
                //AudioManager.instance.Play("Enemy-Hurt");

                //enemy death
                if (enemyStats.curHealth <= 0)
                {
                    enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                    //RollItems();
                    RemoveEnmey(hexCoordOfEnemy);
                    //death audio
                    AudioManager.instance.Play("Deah-Sound");
                }
            }
            AttackIndicators(false);

            Pathfinding.AllPossibleTiles(clickedCoord , playerAttRange, false);

            turnManager.Player_HardAction(playerCoord);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);

        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerPower;
        GlobalVars.players[playerCoord].defense = playerDefense;
    }
    public void Wack(Vector3Int hexCoordOfEnemy , float damage) {
        Debug.Log("Wack Function has started");
        ClearIndicators();
        int ogDodge = GlobalVars.players[playerCoord].dodge;

        Pathfinding.AllPossibleTiles(clickedCoord , 1 , false);

        if(GlobalVars.enemies.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= playerAttRange + 1) {
            Debug.Log("checked if enemy was in range");
            //Get Player and current + future hex objs
            Stats enemyStats = GlobalVars.enemies[clickedCoord];
            GameObject enemyTileObj = GlobalVars.hexagonTile[clickedCoord];
            //checks if player is in smoke and sets the illusionists power as the characters dodge
            if (InSmoke())
            {
                Debug.Log("Checked if player is in smoke");
                GlobalVars.players[clickedCoord].dodge = smokeDodge;
                //sees if the player misses the attack
                if (RollDodge() > GlobalVars.players[clickedCoord].dodge)
                {
                    Debug.Log("Rolled and checked to see in the enemy dodged");
                    //deals damage
                    Debug.Log("Damage was delt");
                    enemyStats.Damage(playerPower);
                    //attack audio
                    AudioManager.instance.Play("Attack");
                    //hit particles
                    Instantiate(hitParticles, worldSpacePos, Quaternion.identity);

                    //enemy death
                    if (enemyStats.curHealth <= 0) {
                        enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                        //RollItems();
                        RemoveEnmey(hexCoordOfEnemy);
                        //death audio
                        AudioManager.instance.Play("Deah-Sound");
                    }
                }
                //resets the players dodge
                GlobalVars.players[clickedCoord].dodge = ogDodge;
            }
            //sees if the player misses the attack
            else if (RollDodge() > GlobalVars.players[playerCoord].dodge)
            {
                Debug.Log("Rolled and checked to see in the enemy dodged");
                //deals damage
                Debug.Log("Damage was delt");
                enemyStats.Damage(playerPower);
                //attack audio
                AudioManager.instance.Play("Attack");
                //hit particles
                Instantiate(hitParticles, worldSpacePos, Quaternion.identity);

                //enemy death
                if (enemyStats.curHealth <= 0)
                {
                    Debug.Log("Enemy was killed");
                    enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                    //RollItems();
                    RemoveEnmey(hexCoordOfEnemy);
                    //death audio
                    AudioManager.instance.Play("Deah-Sound");
                }
            }
            //enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
            AttackIndicators(false);

            turnManager.Player_HardAction(playerCoord);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerPower;
        GlobalVars.players[playerCoord].defense = playerDefense;
    }
    public void Poison() 
    {
        ClearIndicators();
        turnManager.Player_HardAction(playerCoord);

        if (GlobalVars.players[playerCoord].charLevel <= 2)
        {
            if (Vector3Int.Distance(clickedCoord, playerCoord) <= playerAttRange + 1)
            {
                AttackIndicators(false);
                //adds selected tile to a dictionary and sets an indicaotor active
                GlobalVars.poisonTiles.Add(clickedCoord, 2);
                GlobalVars.hexagonTile[clickedCoord].transform.GetChild(4).gameObject.SetActive(true);

                Pathfinding.AllPossibleTiles(clickedCoord, playerAttRange , false);

                //Update player coord
                GlobalVars.players.Remove(clickedCoord);
            }
        }
        else if (GlobalVars.players[playerCoord].charLevel == 3)
        {
            foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(playerCoord, 1, false))
            {
                //adds selected tiles to a dictionary and sets an indicaotors active
                Vector3Int t = temp.Item1;
                GlobalVars.poisonTiles.Add(t, 2);
                foreach (Tuple<Vector3Int, int> coord in Pathfinding.AllPossibleTiles(clickedCoord, 1, false))
                {
                    Vector3Int c = coord.Item1;
                    GlobalVars.smokeTiles.Add(t, 2);
                    GlobalVars.hexagonTile[c].transform.GetChild(4).gameObject.SetActive(true);
                }
            }

            Pathfinding.AllPossibleTiles(clickedCoord, playerAttRange, false);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerPower;
        GlobalVars.players[playerCoord].defense = playerDefense;
    }
    public void SmokeBomb()
    {
        ClearIndicators();
        turnManager.Player_HardAction(playerCoord);

        if (Vector3Int.Distance(clickedCoord, playerCoord) <= playerAttRange + 1)
        {
            AttackIndicators(false);

            foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(clickedCoord, 1, false))
            {
                //adds selected tiles to a dictionary and sets an indicaotors active
                Vector3Int t = temp.Item1;
                GlobalVars.smokeTiles.Add(t, 2);
                GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(true);
            }
            Pathfinding.AllPossibleTiles(clickedCoord, playerAttRange, false);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerPower;
        GlobalVars.players[playerCoord].defense = playerDefense;
    }
    public void Heal(int healthBack) {
        ClearIndicators();
        if (GlobalVars.players[clickedCoord].charLevel == 1) 
        { 
            if(GlobalVars.players.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= 2) {
                //Get Player and current + future hex objs
                Stats playerStats = GlobalVars.players[clickedCoord];
                GameObject playerTileObj = GlobalVars.hexagonTile[clickedCoord];

                //heal player
                playerStats.Heal(healthBack);
                Debug.Log("Player Healed!!");

                //turns off indicator
                HealIndicators(false);

                turnManager.Player_SoftAction(playerCoord);

                //Update player coord
                GlobalVars.players.Remove(clickedCoord);
            }
        }
        else if (GlobalVars.players[clickedCoord].charLevel == 2)
        {
            foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(playerCoord, 1, false))
            {
                //heals all players in a 1 tile range
                Vector3Int t = temp.Item1;
                GlobalVars.players[t].Heal(healthBack);
                Debug.Log("All players in 1 range are healed");
            }

            //turns off indicator
            HealIndicators(false);

            turnManager.Player_SoftAction(playerCoord);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerPower;
        GlobalVars.players[playerCoord].defense = playerDefense;
    }
    public void Move() {
        ClearIndicators();

        int moveRange = turnManager.getMovementLeft(playerCoord);
        List<Tuple<Vector3Int , int>> possibles = Pathfinding.AllPossibleTiles(clickedCoord , moveRange,  true);

        foreach(Tuple<Vector3Int , int> temp in possibles) {
            if(temp.Item1 == clickedCoord && Vector3Int.Distance(clickedCoord , playerCoord) <= moveRange + 1) 
            {
                Movement.movePlayer(playerCoord , clickedCoord);
                MoveIndicators(false);
                TakePoison();

                turnManager.Player_Move(playerCoord , Pathfinding.PathBetweenPoints(clickedCoord , playerCoord, true).Count - 1 , clickedCoord);
                playerCoord = clickedCoord;
            }
        }
    }
    public void Interact() {
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
                    if(tile.Value.sprite.name == "Cliff_Low_comp"){
                        GameObject tileObj = GlobalVars.hexagonTile[tile.Key];
                        tileObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tile.Value.objToChange.sprite;
                        GlobalVars.hexagonTileRefrence[tile.Key] = tile.Value.objToChange;
                    }
                }
            }
        }

        turnManager.Player_HardAction(playerCoord);
    }

    /*********************************
        Tile Indicators
    *********************************/
    public void MoveIndicators(bool onOff) 
    {
        int moveRange = turnManager.getMovementLeft(playerCoord);
        if(moveRange >= 0)
        {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , moveRange - 1 , true)) {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
            }
        }
        else if(moveRange == 0)
        {
            foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(playerCoord, 1 , true))
            {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
            }
        }
    }
    public void AttackIndicators(bool onOff) 
    {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , playerAttRange, false)) {
            Vector3Int t = temp.Item1;
            GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(onOff);
        }
    }
    public void HealIndicators(bool onOff) 
    {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1, false)) {
            Vector3Int t = temp.Item1;
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
        }
    }
    public void InteractIndicators()
    {
        foreach (KeyValuePair<Vector3Int, TileScriptableObjects> temp in GlobalVars.hexagonTileRefrence)
        {
            Vector3Int t = temp.Key;
            if (temp.Value.interactable)
            {
                GlobalVars.hexagonTile[t].transform.GetChild(5).gameObject.SetActive(true);
            }
        }
    }
    public void ClearIndicators()
    {
        foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(playerCoord, 50, false))
        {
            Vector3Int t = temp.Item1;
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(false);
            GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    /*********************************
        UI
    *********************************/
    void UpdatePlayerMenu() {
        statsMenu.SetActive(false);

        Stats stats = GlobalVars.players[playerCoord];
        //updates sprites and health
        selectedPlayerMenu.transform.GetChild(2).GetComponent<Image>().sprite = stats.sprite;
        selectedPlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;
    }
    //sets a menu active and changes all the information
    public void StatsMenu() {
        statsMenu.SetActive(!statsMenu.activeSelf);

        Stats stats = GlobalVars.players[playerCoord];
        charTypeTxt.text = stats.charType.ToString();
        moveTxt.text = "Movement: " + stats.move.ToString();
        powerTxt.text = "Power: " + stats.power.ToString();
        defenseTxt.text = "Hartyness: " + stats.defense.ToString();
        healthTxt.text = "Max Health: " + stats.maxHealth.ToString();
        powerRangeTxt.text = "Power Range: " + stats.attackRange.ToString();
    }
    //sets a menu active and changes all the information
    public void ItemMenu()
    {
        itemMenu.SetActive(!itemMenu.activeSelf);

        items.singleHealTxt.text = "x" + items.singleHealAMT.ToString();
        items.powerBuffTxt.text = "x" + items.powerBuffAMT.ToString();
        items.defenseBuffTxt.text = "x" + items.defenseBuffAMT.ToString();
        items.reviveTxt.text = "x" + items.reviveAMT.ToString();
        items.healScrollTxt.text = "x" + items.healScrollAMT.ToString();
    }
    public void UpdateHealth(float healthOffset) {
        Stats stats = GlobalVars.players[playerCoord];
        stats.curHealth += healthOffset;
        GlobalVars.players[playerCoord] = stats;

        selectedPlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;
    }

    /*********************************
            Item Functions
    *********************************/
    public void SingleHeal()
    {
        //heals the character
        if(items.singleHealAMT != 0)
        {
            Debug.Log("Player was Healed and item was used");
            if (GlobalVars.players[playerCoord].curHealth + 7 >= GlobalVars.players[playerCoord].maxHealth)
            {
                GlobalVars.players[playerCoord].curHealth = GlobalVars.players[playerCoord].maxHealth;
                UpdateHealth(GlobalVars.players[playerCoord].curHealth);
            }
            GlobalVars.players[playerCoord].curHealth += 7;
            UpdateHealth(GlobalVars.players[playerCoord].curHealth);
            AudioManager.instance.Play("Potion");
        }
        items.singleHealAMT--;
    }
    //adds one to the characters power 
    public void PowerBuff()
    {
        if(items.powerBuffAMT != 0)
        {
            GlobalVars.players[playerCoord].power++;
            //end it after a turn?
            AudioManager.instance.Play("Potion");
        }
        items.powerBuffAMT--;
    }
    //adds one to te character defense
    public void DefenseBuff()
    {
        if (items.defenseBuffAMT != 0)
        {
            GlobalVars.players[playerCoord].defense++;
            AudioManager.instance.Play("Potion");
        }
        items.defenseBuffAMT--;
    }
    public void Revive()
    {
        if(items.reviveAMT != 0)
        {

        }
        items.reviveAMT--;
    }
    //heals all characters
    public void HealScroll()
    {
        if(items.healScrollAMT != 0)
        {
            foreach(var health in GlobalVars.players.Values)
            {
                //Debug.Log("all Player was healed");
                if (GlobalVars.players[playerCoord].curHealth + 4 >= GlobalVars.players[playerCoord].maxHealth)
                {
                    GlobalVars.players[playerCoord].curHealth = GlobalVars.players[playerCoord].maxHealth;
                    UpdateHealth(GlobalVars.players[playerCoord].curHealth);
                }
                GlobalVars.players[playerCoord].curHealth += 4;
                UpdateHealth(GlobalVars.players[playerCoord].curHealth);
                AudioManager.instance.Play("Potion");
            }
        }
        items.healScrollAMT--;
    }

    /*********************************
        Other
    *********************************/
    public void RemoveEnmey(Vector3Int enemyHex) {
        GlobalVars.enemies.Remove(enemyHex);        
    }
    //randomizes from 1-100 to see if a player or enemy dodged an attack
    public float RollDodge()
    {
        float dodgeChance = Random.Range(1, 100);
        //Debug.Log(dodgeChance +  " --------------------------------------");
        return dodgeChance;
    }
    public float RollItems()
    {
        float itemChance = Random.Range(1, 100);
        if(itemChance >= 50)
        {
            Debug.Log("Item was dropped");
            float whatItem = Random.Range(1, 100);
            if(itemChance > 0 && itemChance <= GlobalVars.enemies[clickedCoord].singleHealDrop) 
            {
                //drops single heal
                Debug.Log("Single heal was dropped");
            }
            else if (itemChance > 51 && itemChance <= GlobalVars.enemies[clickedCoord].powerBuffDrop)
            {
                //drops power buff
                Debug.Log("Power buff was dropped");
            }
            else if (itemChance > 66 && itemChance <= GlobalVars.enemies[clickedCoord].defBuffDrop)
            {
                //drops defense buff
                Debug.Log("defense buff was dropped");
            }
            else if (itemChance > 81 && itemChance <= GlobalVars.enemies[clickedCoord].reviveDrop)
            {
                //drops revive
                Debug.Log("revive was dropped");
            }
            else if (itemChance > 91 && itemChance <= GlobalVars.enemies[clickedCoord].healScrollDrop)
            {
                //drops healing scroll
                Debug.Log("healing scroll was dropped");
            }
        }
        return 0;
    }
    //Checks how many turns are left for a tile effected by poison, removes it from the dictonary if equals 0
    public void TakePoison()
    {
        foreach(KeyValuePair<Vector3Int, Stats> coord in GlobalVars.players)
        {
            if (!GlobalVars.poisonTiles.ContainsKey(coord.Key))
            {
                continue;
            }
            if (GlobalVars.poisonTiles[coord.Key] != 0)
            {
                GlobalVars.players[coord.Key].Damage(poisonDmg);
                GlobalVars.poisonTiles[coord.Key]--;
            }
            if (GlobalVars.poisonTiles[coord.Key] == 0)
            {
                GlobalVars.poisonTiles.Remove(coord.Key);
            }
        }
    }
    //Checks how many turns are left for a tile effected by smoke, removes it from the dictonary if equals 0
    public bool InSmoke()
    {
        foreach (KeyValuePair<Vector3Int, Stats> coord in GlobalVars.players)
        {
            if (!GlobalVars.smokeTiles.ContainsKey(coord.Key))
            {
                continue;
            }
            if (GlobalVars.smokeTiles[coord.Key] != 0)
            {
                GlobalVars.smokeTiles[coord.Key]--;
                return true;
            }
            if (GlobalVars.smokeTiles[coord.Key] == 0)
            {
                GlobalVars.smokeTiles.Remove(coord.Key);
            }
        }
        return false;
    }
    //gets the position of a mouse click
    public Vector3Int GetPosition() {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x , mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D , Vector2.zero);
            if(hit.collider != null) {
                //Debug.Log(hit.collider.gameObject.transform.position);
                clickedCoord = hit.collider.transform.GetComponent<HexObjInfo>().hexCoord;
                return clickedCoord;
            }
            return Vector3Int.zero;
        }
        return Vector3Int.zero;
    }
    public void UpdateHealthBar(float curValue, float maxValue)
    {
        slider.value = curValue / maxValue;
    }
}