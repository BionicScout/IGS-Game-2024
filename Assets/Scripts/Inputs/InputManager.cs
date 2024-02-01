using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public bool normMode;
    public bool shootMode;
    public bool wackMode;
    public bool moveMode;
    public int playerMove;
    public int playerPower;
    static Vector3Int clickedCoord;
    PlayerStats playerStats;
    EnemyStats enemyStats;
    HexObjInfo hexObjInfo;
    Movement movement;


    void Start()
    {
        normMode = true;
        shootMode = false;
        wackMode = false;
        moveMode = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Player")) 
                {
                    playerMove = gameObject.GetComponent<PlayerStats>().move;
                    Debug.Log(playerMove);
                    playerPower = gameObject.GetComponent<PlayerStats>().power;
                    Debug.Log(playerPower);
                }
                else
                {
                    GetPosition();
                }

            }
        }

        if (shootMode /*&& clicked on this player*/)
        {
            Shoot(clickedCoord, 2);
            //final thing
            normMode = true;
        }
        if (wackMode)
        {
            Wack(clickedCoord, 2);
            //final thing
            normMode = true;
        }
        if (moveMode)
        {
            Move(clickedCoord, playerStats.move);
            //final thing
            normMode = true;
        }
    }
    //functions buttons will use
    public bool SetShoot()
    {
        shootMode = true;
        return shootMode;
    }
    public bool SetWack()
    {
        wackMode = true;
        return wackMode;
    }
    public bool SetMove()
    {
        moveMode = true;
        return moveMode;
    }


    public void Shoot(Vector3Int hexCoordOfEnemy, float damage)
    {
        Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, playerStats.move);

        if (GlobalVars.enemies.ContainsKey(clickedCoord))
        {
            enemyStats.TakeDamage(playerStats.power);
        }
    }

    public void Wack(Vector3Int hexCoordOfEnemy, float damage)
    {
        Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, 1);

        if (GlobalVars.enemies.ContainsKey(clickedCoord))
        {
            enemyStats.TakeDamage(playerStats.power);
        }
    }

    public void Move(Vector3Int hexCoodOfEnemy, int range)
    {
        List<Tuple<Vector3Int, int>> possibles = Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, playerStats.move);

        foreach (Tuple<Vector3Int, int> temp in possibles) {

            if (temp.Item1 == clickedCoord)
            {
                //movement.movePlayer(, hexObjInfo.hexCoord);
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
