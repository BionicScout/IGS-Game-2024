using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{

    enum modes {
        normal = 0,
        shoot = 1,
        attack = 2,
        move = 3
    }

    modes inputMode; 




    public int playerMove;
    public int playerPower;
    public bool clickedUI = false;
    static Vector3Int clickedCoord, playerCoord;
    PlayerStats playerStats;
    EnemyStats enemyStats;
    //HexObjInfo hexObjInfo;
    Movement movement;


    void Start() {
        inputMode = modes.normal;
    }
    void Update()
    {

        if(clickedUI) {
            clickedUI = false;
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                clickedCoord = hit.collider.gameObject.transform.GetComponent<HexObjInfo>().hexCoord;
                if (GlobalVars.players.ContainsKey(clickedCoord)) 
                {
                    playerCoord = clickedCoord;

                    playerMove = GlobalVars.players[clickedCoord].move;
                    Debug.Log(playerMove);
                    playerPower = GlobalVars.players[clickedCoord].power;
                    Debug.Log(playerPower);
                }
                else
                {
                    GetPosition();
                }

            }

            if(inputMode == modes.shoot /*&& clicked on this player*/) {
                Shoot(clickedCoord , 2);
                //final thing
                inputMode = modes.normal;
            }
            if(inputMode == modes.attack) {
                Wack(clickedCoord , 2);
                //final thing
                inputMode = modes.normal;
            }
            if(inputMode == modes.move) {
                Move(clickedCoord , playerMove);
                //final thing
                inputMode = modes.normal;
            }
        }
    }
    //functions buttons will use
    public void SetShoot()
    {
        inputMode = modes.shoot;
        clickedUI = true;
    }
    public void SetWack()
    {
        inputMode = modes.attack;
        clickedUI = true;
    }
    public void SetMove()
    {
        inputMode = modes.move;
        clickedUI = true;
    }


    public void Shoot(Vector3Int hexCoordOfEnemy, float damage)
    {
        Pathfinding.AllPossibleTiles(clickedCoord , playerMove);

        if (GlobalVars.enemies.ContainsKey(clickedCoord))
        {
            //Get Player and current + future hex objs
            Stats enemyStats = GlobalVars.enemies[clickedCoord];
            GameObject enemyTileObj = GlobalVars.hexagonTile[clickedCoord];

            //Update Sprite
            enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
    }

    public void Wack(Vector3Int hexCoordOfEnemy, float damage)
    {
        Pathfinding.AllPossibleTiles(clickedCoord , 1);

        if (GlobalVars.enemies.ContainsKey(clickedCoord))
        {
            //Get Player and current + future hex objs
            Stats enemyStats = GlobalVars.enemies[clickedCoord];
            GameObject enemyTileObj = GlobalVars.hexagonTile[clickedCoord];

            //Update Sprite
            enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;

            //Update player coord
            GlobalVars.players.Remove(clickedCoord);
        }
    }

    public void Move(Vector3Int hexCoodOfEnemy, int range)
    {
        List<Tuple<Vector3Int, int>> possibles = Pathfinding.AllPossibleTiles(clickedCoord, playerMove);

        foreach (Tuple<Vector3Int, int> temp in possibles) {

            if (temp.Item1 == clickedCoord)
            {
                Movement.movePlayer(playerCoord, clickedCoord);
            }
        }
    }


    public Vector3Int GetPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.transform.position);
                clickedCoord = hit.collider.transform.GetComponent<HexObjInfo>().hexCoord;
                return clickedCoord;
            }
            return Vector3Int.zero;
        }
        return Vector3Int.zero;
    }
}

    //public UnityEvent<Vector3> PointerClick;

    //void Update()
    //{
    //    DetectMouseClick();
    //}

    //private void DetectMouseClick()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 mousePos = Input.mousePosition;
    //        PointerClick?.Invoke(mousePos);
    //    }
    //}
