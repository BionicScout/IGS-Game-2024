using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;


public class InputManager : MonoBehaviour {
    public enum modes {
        undefined = -1,
        normal = 0,
        attack = 1,
        move = 2,
        AOE = 3,
        heal = 4,
        interact = 5
    }

    [HideInInspector]
    public modes inputMode;


    public bool clickedUI = false;
    public Items items;


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
    public GameObject radioMenu;



    //HexObjInfo hexObjInfo;
    [Header("Other")]
    public Camera cam;

    TileIndicators tileIndicators;
    PlayerAction playerActions;
    CombatUI combatUI;




    public Vector3Int getClickedCoord() {
        return clickedCoord;
    }

    public Vector3Int getPlayerCoord() {
        return playerCoord;
    }


    /*********************************
        Start and Update
    *********************************/
    void Start() {
        inputMode = modes.normal;
        selectedPlayerMenu.SetActive(false);
        statsMenu.SetActive(false);
        itemMenu.SetActive(false);

        tileIndicators = FindAnyObjectByType<TileIndicators>();
        playerActions = FindAnyObjectByType<PlayerAction>();
        combatUI = FindAnyObjectByType<CombatUI>();
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
            tileIndicators.ClearIndicators(playerCoord);
            GlobalVars.hexagonTile[clickedCoord].transform.GetChild(5).gameObject.SetActive(true);

            worldSpacePos = mousePos;

            //if the player clicked on a collider the if their is a character on that tile
            if(hit.collider != null) {
                if (GlobalVars.players.ContainsKey(clickedCoord)) {
                    playerCoord = clickedCoord;
                    combatUI.UpdatePlayerMenu();

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
                    tileIndicators.ClearIndicators(playerCoord);

                }
            }
            //triggers the specific functions from the buttons
            if(inputMode == modes.attack) {
                if (GlobalVars.players[playerCoord].charType == "Swordsman" || GlobalVars.players[playerCoord].charType == "Spearman" || GlobalVars.players[playerCoord].charType == "Paladin")
                {
                    Debug.Log("Wack function was called");
                    playerActions.Shoot(clickedCoord, playerCoord);
                    inputMode = modes.normal;
                }
                else if (GlobalVars.players[playerCoord].charType == "Archer" || GlobalVars.players[playerCoord].charType == "Crossbowman")
                {
                    Debug.Log("Shoot function was called");
                    playerActions.Shoot(clickedCoord, playerCoord);
                    inputMode = modes.normal;
                }
                else if(GlobalVars.players[playerCoord].charType == "Alchemist")
                {
                    playerActions.Poison(clickedCoord, playerCoord);
                    inputMode = modes.normal;
                }
                else if (GlobalVars.players[clickedCoord].charType == "Cleric")
                {
                    playerActions.Heal(GlobalVars.players[clickedCoord].power, clickedCoord , playerCoord);
                    inputMode = modes.normal;
                }
                else if (GlobalVars.players[playerCoord].charType == "Illusionist")
                {
                    playerActions.SmokeBomb(clickedCoord , playerCoord);
                    inputMode = modes.normal;
                }
            }
            else if(inputMode == modes.move) {
                playerActions.Move(clickedCoord , playerCoord);
                inputMode = modes.normal;
            }
            else if (inputMode == modes.AOE)
            {
                playerActions.SmokeBomb(clickedCoord , playerCoord);
                inputMode = modes.normal;
            }
            else if (inputMode == modes.heal)
            {
                playerActions.Heal(playerPower, clickedCoord , playerCoord);
                inputMode = modes.normal;
            }
            else if(inputMode == modes.interact)
            {
                playerActions.Interact(clickedCoord , playerCoord);
                inputMode = modes.normal;
            }
        }

        //Debug stuff
        if(Input.GetKeyDown(KeyCode.G)) {
            combatUI.UpdateHealth(-1);
        }
        if(Input.GetKeyDown(KeyCode.H)) {
            combatUI.UpdateHealth(1);
        }
    }


    /*********************************
        Other
    *********************************/

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
                combatUI.Player_UpdateHealth(coord.Key);
                GlobalVars.poisonTiles[coord.Key]--;
            }
            if (GlobalVars.poisonTiles[coord.Key] == 0)
            {
                GlobalVars.poisonTiles.Remove(coord.Key);
            }
        }
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





    public void selectPlayer(int index) {
        string charName = GlobalVars.choosenPlayers[index].charName;

       
        foreach(KeyValuePair<Vector3Int, Stats> player in GlobalVars.players) {

            if(player.Value.charName == charName) {
                playerCoord = player.Key;
                clickedCoord = player.Key;

                combatUI.UpdatePlayerMenu();

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
                tileIndicators.ClearIndicators(playerCoord);
            }
        }
    }
}