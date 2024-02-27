using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

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

    public int playerMove, playerPower, playerAttRange, playerDefense, playerMaxHealth;
    public bool clickedUI = false;
    //public float stopTime;
    static Vector3Int clickedCoord, playerCoord, enemyCoord, mouseCoord;
    public GameObject meleeMenu, rangeMenu, magicMenu, statsMenu, hitParticles;
    public TextMeshProUGUI moveTxt, powerTxt, defenseTxt, healthTxt, powerRangeTxt;

    //HexObjInfo hexObjInfo;

    TurnManager turnManager;


    void Start() {
        inputMode = modes.normal;
        meleeMenu.SetActive(false);
        rangeMenu.SetActive(false);
        magicMenu.SetActive(false);
        statsMenu.SetActive(false);
        turnManager = FindAnyObjectByType<TurnManager>();
    }
    void Update() {

        if(clickedUI) {
            clickedUI = false;
            return;
        }
        //stopTime += Time.deltaTime;

        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x , mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D , Vector2.zero);
            if(hit.collider != null) {
                GlobalVars.hexagonTile[clickedCoord].transform.GetChild(5).gameObject.SetActive(false);
                if (GlobalVars.players.ContainsKey(clickedCoord)) {
                    //checks what menu needs to be opened
                    if (GlobalVars.players[clickedCoord].charType == "Melee")
                    {
                        meleeMenu.SetActive(true);
                        rangeMenu.SetActive(false);
                        magicMenu.SetActive(false);
                    }
                    if (GlobalVars.players[clickedCoord].charType == "Range")
                    {
                        rangeMenu.SetActive(true);
                        meleeMenu.SetActive(false);
                        magicMenu.SetActive(false);
                    }
                    if (GlobalVars.players[clickedCoord].charType == "Magic")
                    {
                        magicMenu.SetActive(true);
                        meleeMenu.SetActive(false);
                        rangeMenu.SetActive(false);
                    }
                    playerCoord = clickedCoord;

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
                else {
                    GetPosition();
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
            AudioManager.instance.Play("Attack");
        }
        if(Input.GetKeyDown(KeyCode.H)) {
            AudioManager.instance.Play("Death-Bells");
        }
    }
    //functions buttons will use
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
    public void SetAOE()
    {
        inputMode = modes.AOE;
        clickedUI = true;
    }
    public void SetHeal()
    {
        ShootIndicators(false);
        WackIndicators(false);
        HealIndicators(true);
        inputMode = modes.heal;
        clickedUI= true;
    }
    public void StatsMenu(bool onOff)
    {
        statsMenu.SetActive(onOff);
        moveTxt.text = "Movement: " + playerMove.ToString();
        powerTxt.text = "Power: " + playerPower.ToString();
        defenseTxt.text = "Hartyness: " + playerDefense.ToString();
        healthTxt.text = "Max Health: " + playerMaxHealth.ToString();
        powerRangeTxt.text = "Power Range: " + playerAttRange.ToString();
   
    }
    public void Items()
    {
        Debug.Log("Item menu opened");
    }
    public void Interact()
    {
        Debug.Log("INTERACT");
        turnManager.Player_HardAction(playerCoord);
    }

    //functions for turning all the indicators on
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
    public void HealIndicators(bool onOff)
    {
        foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(playerCoord, 1))
        {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
        }
    }
    public void ClickedIndicatorsOff()
    {
        foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(clickedCoord, 10))
        {
            Vector3Int t = temp.Item1;
            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
            GlobalVars.hexagonTile[t].transform.GetChild(5).gameObject.SetActive(false);
        }
    }
    //public void AOEIndeicators(bool onOff)
    //{
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

    //    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //    if(hit.collider != null)
    //    {
    //        mouseCoord = hit.collider.gameObject.transform.GetComponent<HexObjInfo>().hexCoord;
    //        foreach (Tuple<Vector3Int, int> temp in Pathfinding.AllPossibleTiles(mouseCoord, playerAttRange))
    //        {
    //            Vector3Int t = temp.Item1;
    //            //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
    //            GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(onOff);
    //        }
    //    }
    //}

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
            //hit particles
            Instantiate(hitParticles, enemyCoord, Quaternion.identity);
            if (enemyStats.curHealth <= 0) {
                enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
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
            //hit particles
            Instantiate(hitParticles, enemyCoord, Quaternion.identity);
            if (enemyStats.curHealth <= 0) {
                enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
            }
            enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
            WackIndicators(false);

            turnManager.Player_SoftAction(playerCoord);

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);

        }
    }
    public void MagicAOE()
    {

    }
    public void Heal(int healthBack)
    {
        ShootIndicators(false);
        WackIndicators(false);

        if (GlobalVars.players.ContainsKey(clickedCoord) && Vector3Int.Distance(clickedCoord, playerCoord) <= 2)
        {
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

                turnManager.Player_Move(playerCoord, Pathfinding.PathBetweenPoints(clickedCoord, playerCoord).Count - 1, clickedCoord);
                playerCoord = clickedCoord;
            }
        }
    }

    public void PlayerTurn(bool isPlayerTurn) {

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

