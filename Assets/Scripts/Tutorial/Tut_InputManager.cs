using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;


public class Tut_InputManager : MonoBehaviour {
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
    public GameObject radioMenu;

    [Header("Text")]
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI powerTxt;
    public TextMeshProUGUI defenseTxt;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI powerRangeTxt;
    public TextMeshProUGUI charTypeTxt;


    //HexObjInfo hexObjInfo;

    public Camera cam;

    TurnManager turnManager;

    /*********************************
        Start and Update
    *********************************/
    void Start() {
        inputMode = modes.normal;
        selectedPlayerMenu.SetActive(false);
        statsMenu.SetActive(false);
        itemMenu.SetActive(false);
        turnManager = FindAnyObjectByType<TurnManager>();
        Debug.Log("Hello World");

        selectPlayer(0);
        //moveRadioWheel();

        //Unit Sector Images
        Transform selectImage = selectedPlayerMenu.transform.GetChild(0).GetChild(0).GetChild(0);

        Transform characterSelectButtons = selectedPlayerMenu.transform.GetChild(1);
        Debug.Log(GlobalVars.players.Count);
        int i = 0;
        foreach(KeyValuePair<Vector3Int , Stats> player in GlobalVars.players) {
            if(i == 0) {
                selectImage.GetComponent<Image>().sprite = player.Value.squareSprite; 
            }

            Debug.Log(i);
            Image image = characterSelectButtons.GetChild(i).GetChild(0).GetComponent<Image>();
            image.sprite = player.Value.squareSprite;
            i++;
        }
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
            else if(inputMode == modes.move) {
                Move();
                inputMode = modes.normal;
            }
            else if (inputMode == modes.AOE)
            {
                SmokeBomb();
                inputMode = modes.normal;
            }
            else if (inputMode == modes.heal)
            {
                Heal(playerPower);
                inputMode = modes.normal;
            }
            else if(inputMode == modes.interact)
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
        if(turnManager.getMovementLeft(playerCoord) <= 0) {
            return;
        }

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
        if(turnManager.getActionUse(playerCoord))
        {
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
                    Enemy_UpdateHealth(clickedCoord);
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
    }
    public void Wack(Vector3Int hexCoordOfEnemy , float damage) {
        if (turnManager.getActionUse(playerCoord))
        {

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
                    Enemy_UpdateHealth(clickedCoord);
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
    }
    public void Poison() 
    {
        if (turnManager.getActionUse(playerCoord))
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
                    GlobalVars.poisonTiles.Add(t, 2);
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
    }
    public void SmokeBomb()
    {
        if (turnManager.getActionUse(playerCoord))
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
    }
    public void Heal(int healthBack) {
        if (turnManager.getActionUse(playerCoord))
        {
        ClearIndicators();
        if (GlobalVars.players[clickedCoord].charLevel == 1) 
        { 
            if(GlobalVars.players.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= 2) {
                //Get Player and current + future hex objs
                Stats playerStats = GlobalVars.players[clickedCoord];
                GameObject playerTileObj = GlobalVars.hexagonTile[clickedCoord];

                //heal player
                playerStats.Heal(healthBack);
                Player_UpdateHealth(clickedCoord);
                Debug.Log("Player Healed!!");

                //turns off indicator
                HealIndicators(false);

                turnManager.Player_HardAction(playerCoord);

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

            turnManager.Player_HardAction(playerCoord);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
        //resets a players power and defense incase they used an item
        GlobalVars.players[playerCoord].power = playerPower;
        GlobalVars.players[playerCoord].defense = playerDefense;

        }
    }
    public void Move() {
        ClearIndicators();

        Debug.Log(Pathfinding.PathBetweenPoints(clickedCoord , playerCoord , false).Count);

        int moveRange = turnManager.getMovementLeft(playerCoord);
        List<Tuple<Vector3Int , int>> possibles = Pathfinding.AllPossibleTiles(clickedCoord , moveRange,  true);


        foreach(Tuple<Vector3Int , int> temp in possibles) {
            Debug.Log("--------------------");
            int dist = Pathfinding.PathBetweenPoints(temp.Item1 , playerCoord , false).Count - 1;

            foreach(Vector3Int pos in Pathfinding.PathBetweenPoints(temp.Item1 , playerCoord , true)) {
                Debug.Log(pos);
            }

            Debug.Log(dist);
            //Debug.Log();

            if(temp.Item1 == clickedCoord && dist <= moveRange + 1) 
            {
                Debug.Log("Hi");
                Movement.movePlayer(playerCoord , clickedCoord);
                //moveRadioWheel();
                MoveIndicators(false);
                TakePoison();

                turnManager.Player_Move(playerCoord , dist, clickedCoord);
                playerCoord = clickedCoord;
                break;
            }
        }
    }
    public void Interact() {
        if (turnManager.getActionUse(playerCoord))
        {

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

        Debug.Log(GlobalVars.availableHexes.Count);

        Stats stats = GlobalVars.players[playerCoord];
        //updates sprites and health
        selectedPlayerMenu.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = stats.squareSprite;
        //selectedPlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;
    }
    //sets a menu active and changes all the information
    public void StatsMenu() {
        statsMenu.SetActive(!statsMenu.activeSelf);

        Stats stats = GlobalVars.players[playerCoord];
        charTypeTxt.text = stats.charType.ToString();
        moveTxt.text = stats.move.ToString();
        powerTxt.text =  stats.power.ToString();
        defenseTxt.text = stats.defense.ToString();
        healthTxt.text = stats.maxHealth.ToString();
        powerRangeTxt.text = stats.attackRange.ToString();
    }
    //sets a menu active and changes all the information
    public void ItemMenu()
    {
        itemMenu.SetActive(!itemMenu.activeSelf);

        items.singleHealTxt.text = "x" + items.singleHealAMT.ToString();
        items.powerBuffTxt.text = "x" + items.powerBuffAMT.ToString();
        items.defenseBuffTxt.text = "x" + items.defenseBuffAMT.ToString();
        items.healScrollTxt.text = "x" + items.healScrollAMT.ToString();
    }
    public void UpdateHealth(float healthOffset) {
        Stats stats = GlobalVars.players[playerCoord];
        stats.curHealth += healthOffset;
        GlobalVars.players[playerCoord] = stats;
        Player_UpdateHealth(playerCoord);

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
            Player_UpdateHealth(playerCoord);
            items.singleHealAMT--;
            turnManager.Player_PotionAction(playerCoord);
        }

        ItemMenu();
    }
    //adds one to the characters power 
    public void PowerBuff()
    {
        items.powerBuffUsed = true;
        if(items.powerBuffAMT != 0)
        {
            GlobalVars.players[playerCoord].power++;
            //end it after a turn?
            AudioManager.instance.Play("Potion");
            items.powerBuffAMT--;
            turnManager.Player_PotionAction(playerCoord);
        }

        ItemMenu();
    }
    //adds one to te character defense
    public void DefenseBuff()
    {
        items.defenseBuffUsed = true;
        if (items.defenseBuffAMT != 0)
        {
            GlobalVars.players[playerCoord].defense++;
            AudioManager.instance.Play("Potion");
            items.defenseBuffAMT--;
            turnManager.Player_PotionAction(playerCoord);
        }

        ItemMenu();
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
                Player_UpdateHealth(playerCoord);
            }

            turnManager.Player_PotionAction(playerCoord);
            items.healScrollAMT--;
        }

        ItemMenu();
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
            if (GlobalVars.poisonTiles[coord.Key] != 0) {
                GlobalVars.players[coord.Key].Damage(poisonDmg);
                Player_UpdateHealth(coord.Key);
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










    public void moveRadioWheel() {
        Vector3 screenPosition = cam.WorldToScreenPoint(GlobalVars.hexagonTile[playerCoord].transform.position);
        radioMenu.transform.position = screenPosition;
    }

    public void toggleRadioMenu() {
        radioMenu.SetActive(radioMenu.activeSelf);
    }

    public void selectPlayer(int index) {
        string charName = GlobalVars.choosenPlayers[index].charName;

       
        foreach(KeyValuePair<Vector3Int, Stats> player in GlobalVars.players) {

            if(player.Value.charName == charName) {
                playerCoord = player.Key;
                clickedCoord = player.Key;

                UpdatePlayerMenu();

                //gets players stats annd stores them
                playerMove = GlobalVars.players[clickedCoord].move;
                playerPower = GlobalVars.players[clickedCoord].power;
                playerAttRange = GlobalVars.players[clickedCoord].attackRange;
                playerDefense = GlobalVars.players[clickedCoord].defense;
                playerMaxHealth = GlobalVars.players[clickedCoord].maxHealth;

                //Characters specific tings that need to be stored
                if(GlobalVars.players[clickedCoord].charType == "Alchemist") {
                    poisonDmg = GlobalVars.players[clickedCoord].power;
                }
                if(GlobalVars.players[clickedCoord].charType == "Illusionist") {
                    smokeDodge = GlobalVars.players[clickedCoord].power;
                }

                //sets all indicators false when players are clicked
                ClearIndicators();
            }
        }
    }
}