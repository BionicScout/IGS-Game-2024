using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class InputManager : MonoBehaviour
{
    private bool normMode;
    private bool shootMode;
    private bool wackMode;
    private bool moveMode;
    static Vector3Int clickedCoord;
    private Vector3Int clickedCord;
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
        GetPosition();

        if (shootMode /*&& clicked on this player*/)
        {
            //final thing
            normMode = true;
        }
        if (wackMode)
        {
            //final thing
            normMode = true;
        }
        if (moveMode)
        {
            //final thing
            normMode = true;
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
        Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, playerStats.move);

        //if (GlobalVars.availableHexes.ContainsKey(clickedCoord))
        {
            movement.moveTile(range);
        }
    }

    public void Move(Vector3Int hexCoodOfEnemy, float range)
    {
        Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, playerStats.move);

        //if ()
        {
            movement.moveTile(playerStats.move);
        }
    }

}
