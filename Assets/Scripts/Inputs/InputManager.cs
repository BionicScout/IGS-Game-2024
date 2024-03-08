using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class InputManager : MonoBehaviour {
    enum modes {
        normal = 0,
        shoot = 1,
        attack = 2,
        move = 3,
        AOE = 4,
        heal = 5
    }

    modes inputMode;
    public Items items;

    [SerializeField]
    GameObject hitParticles;

    public int playerMove, playerPower, playerAttRange, playerDefense, playerMaxHealth;
    public bool clickedUI = false;
    //public float stopTime;
    static Vector3Int clickedCoord, playerCoord, enemyCoord, mouseCoord;
    Vector3 worldSpacePos;
    public GameObject selectedPlayerMenu, statsMenu, itemMenu;
    public TextMeshProUGUI moveTxt, powerTxt, defenseTxt, healthTxt, powerRangeTxt;



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
        //singleHealAMT = 2;
        //powerBuffAMT = 3;
        //defenseBuffAMT = 2;
        //reviveAMT = 2;
        //healScrollAMT = 1;
    }
    void Update() {

        if(clickedUI) {
            clickedUI = false;
            return;
        }
        //stopTime += Time.deltaTime;

        GetPosition();

        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x , mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D , Vector2.zero);

            worldSpacePos = mousePos;

            if(hit.collider != null) {
                GlobalVars.hexagonTile[clickedCoord].transform.GetChild(5).gameObject.SetActive(false);
                if (GlobalVars.players.ContainsKey(clickedCoord)) {
                    playerCoord = clickedCoord;
                    UpdatePlayerMenu();

                    //gets players stats annd stores them
                    playerMove = GlobalVars.players[clickedCoord].move;
                    playerPower = GlobalVars.players[clickedCoord].power;
                    playerAttRange = GlobalVars.players[clickedCoord].attackRange;
                    playerDefense = GlobalVars.players[clickedCoord].defense;
                    playerMaxHealth = GlobalVars.players[clickedCoord].maxHealth;

                    //sets all indicators false when players are clicked
                    MoveIndicators(false);
                    WackIndicators(false);
                    ShootIndicators(false);
                    HealIndicators(false);

                }
            }

            if(inputMode == modes.shoot /*&& clicked on this player*/) {
                Shoot(clickedCoord , 2);
                inputMode = modes.normal;
            }
            if(inputMode == modes.attack) {
                Wack(clickedCoord , 2);
                inputMode = modes.normal;
            }
            if(inputMode == modes.move) {
                Move();
                inputMode = modes.normal;
            }
            if (inputMode == modes.AOE)
            {
                mouseCoord = hit.collider.gameObject.transform.GetComponent<HexObjInfo>().hexCoord;
                foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(mouseCoord, playerAttRange))
                {
                    Vector3Int t = temp.Item1;
                    //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
                    GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(true);
                }
            }
            if (inputMode == modes.heal)
            {
                Heal(playerPower);
                inputMode = modes.normal;
            }
        }


        if(Input.GetKeyDown(KeyCode.G)) {
            UpdateHealth(-1);
        }
        if(Input.GetKeyDown(KeyCode.H)) {
            UpdateHealth(1);
        }

        //if(Input.GetKeyDown(KeyCode.G)) {
        //    AudioManager.instance.Play("Attack");
        //}
        //if(Input.GetKeyDown(KeyCode.H)) {
        //    AudioManager.instance.Play("Death-Bells");
        //}
    }



    /*********************************
        Update Input Mode
    *********************************/
    public void SetShoot() {
        MoveIndicators(false);
        HealIndicators(false);
        ShootIndicators(true);
        inputMode = modes.shoot;
        clickedUI = true;
    }
    public void SetWack() {
        MoveIndicators(false);
        HealIndicators(false);
        WackIndicators(true);
        inputMode = modes.attack;
        clickedUI = true;
    }
    public void SetMove() {
        ShootIndicators(false);
        WackIndicators(false);
        HealIndicators(false);
        MoveIndicators(true);
        inputMode = modes.move;
        clickedUI = true;
    }
    public void SetAOE() {
        MoveIndicators(false);
        HealIndicators(false);
        //AOEIndicators(true);
        inputMode = modes.AOE;
        clickedUI = true;
    }
    public void SetHeal() {
        ShootIndicators(false);
        WackIndicators(false);
        HealIndicators(true);
        inputMode = modes.heal;
        clickedUI = true;
    }

    /*********************************
        Actions
    *********************************/
    public void Shoot(Vector3Int hexCoordOfEnemy , float damage) {
        WackIndicators(false);
        MoveIndicators(false);
        HealIndicators(false);

        turnManager.Player_HardAction(playerCoord);

        if(GlobalVars.enemies.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= playerAttRange + 1) {
            //Get Player and current + future hex objs
            Stats enemyStats = GlobalVars.enemies[clickedCoord];
            GameObject enemyTileObj = GlobalVars.hexagonTile[clickedCoord];

            //deals damage
            enemyStats.Damage(playerPower);
            //attack audio
            AudioManager.instance.Play("Attack");
            //hit particles
            Instantiate(hitParticles, worldSpacePos, Quaternion.identity);

            //enemy death
            if (enemyStats.curHealth <= 0) {
                enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                RemoveEnmey(hexCoordOfEnemy);
                //death audio
                AudioManager.instance.Play("Deah-Sound");
            }

            ShootIndicators(false);

            Pathfinding.AllPossibleTiles(clickedCoord , playerAttRange);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);

        }

    }
    public void Wack(Vector3Int hexCoordOfEnemy , float damage) {
        MoveIndicators(false);
        ShootIndicators(false);
        HealIndicators(false);

        Pathfinding.AllPossibleTiles(clickedCoord , 1);

        if(GlobalVars.enemies.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord , playerCoord) <= 2) {
            //Get Player and current + future hex objs
            Stats enemyStats = GlobalVars.enemies[clickedCoord];
            GameObject enemyTileObj = GlobalVars.hexagonTile[clickedCoord];

            //Deals damage
            enemyStats.Damage(playerPower);
            //attack audio
            AudioManager.instance.Play("Attack");
            //hit particles
            Instantiate(hitParticles, worldSpacePos, Quaternion.identity);

            //enemy death
            if (enemyStats.curHealth <= 0) {
                enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
                RemoveEnmey(hexCoordOfEnemy);
                //death audio
                AudioManager.instance.Play("Deah-Sound");
            }
            //enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
            WackIndicators(false);

            turnManager.Player_HardAction(playerCoord);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);

        }
    }
    public void MagicAOE() {

    }
    public void Heal(int healthBack) {
        ShootIndicators(false);
        WackIndicators(false);

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

    public void Move() {
        ShootIndicators(false);
        WackIndicators(false);

        int moveRange = turnManager.getMovementLeft(playerCoord);
        List<Tuple<Vector3Int , int>> possibles = Pathfinding.AllPossibleTiles(clickedCoord , moveRange);

        foreach(Tuple<Vector3Int , int> temp in possibles) {
            if(temp.Item1 == clickedCoord && Vector3Int.Distance(clickedCoord , playerCoord) <= moveRange + 1) {
                //Debug.Log("Tile distance: " + Vector3Int.Distance(clickedCoord , playerCoord));
                //Debug.Log("This is Item1 " + temp.Item1);

                Movement.movePlayer(playerCoord , clickedCoord);
                MoveIndicators(false);

                turnManager.Player_Move(playerCoord , Pathfinding.PathBetweenPoints(clickedCoord , playerCoord).Count - 1 , clickedCoord);
                playerCoord = clickedCoord;
            }
        }
    }

    public void Items() {
        Debug.Log("Item menu opened");
    }
    public void Interact() {
        Debug.Log("INTERACT");
        turnManager.Player_HardAction(playerCoord);
    }



    /*********************************
        Tile Indicators
    *********************************/
    public void MoveIndicators(bool onOff) {
        //int moveRange = turnManager.getMovementLeft(playerCoord);

        int moveRange = turnManager.getMovementLeft(playerCoord);
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , moveRange)) {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
        }
    }
    public void WackIndicators(bool onOff) {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1)) {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(onOff);
        }
    }
    public void ShootIndicators(bool onOff) {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , playerAttRange)) {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(onOff);
        }
    }
    public void HealIndicators(bool onOff) {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1)) {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
        }
    }
    public void ClickedIndicatorsOff() {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(clickedCoord , 10)) {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(5).gameObject.SetActive(false);
        }
    }
    public void AOEIndicators(bool onOff)
    {
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        //if (hit.collider != null)
        //{
        //    mouseCoord = hit.collider.gameObject.transform.GetComponent<HexObjInfo>().hexCoord;
        //    foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(mouseCoord, playerAttRange))
        //    {
        //        Vector3Int t = temp.Item1;
        //        //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
        //        GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(onOff);
        //    }
        //}
    }



    /*********************************
        UI
    *********************************/
    void UpdatePlayerMenu() {
        statsMenu.SetActive(false);

        Stats stats = GlobalVars.players[playerCoord];
        selectedPlayerMenu.transform.GetChild(2).GetComponent<Image>().sprite = stats.sprite;
        selectedPlayerMenu.transform.GetChild(1).GetComponent<Slider>().value = (float)stats.curHealth / stats.maxHealth;
    }

    public void StatsMenu() {
        statsMenu.SetActive(!statsMenu.activeSelf);

        Stats stats = GlobalVars.players[playerCoord];
        moveTxt.text = "Movement: " + stats.move.ToString();
        powerTxt.text = "Power: " + stats.power.ToString();
        defenseTxt.text = "Hartyness: " + stats.defense.ToString();
        healthTxt.text = "Max Health: " + stats.maxHealth.ToString();
        powerRangeTxt.text = "Power Range: " + stats.attackRange.ToString();
    }
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
        }
        items.singleHealAMT--;
    }
    public void PowerBuff()
    {
        if(items.powerBuffAMT != 0)
        {
            Debug.Log("");
            GlobalVars.players[playerCoord].power++;
            //end it after a turn?
        }
        items.powerBuffAMT--;
    }
    public void DefenseBuff()
    {
        if (items.defenseBuffAMT != 0)
        {
            GlobalVars.players[playerCoord].defense++;
            //end it after a turn?
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

}

