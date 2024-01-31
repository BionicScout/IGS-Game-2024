using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    private bool normMode;
    private bool shootMode;
    private bool wackMode;
    private bool moveMode;
    PlayerStats playerStats;
    HexObjInfo hexObjInfo;

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

        if(shootMode) 
        {
            //final thing
            normMode = true;
        }
        if(wackMode)
        {
            //final thing
            normMode = true;
        }
        if(moveMode) 
        {
            //final thing
            normMode = true;
        }
    }
    public void GetPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.transform.position);
                //return hit.collider.gameObject.transform.position;
            }
        }
    }

    public void Shoot(Vector3Int hexCoordOfEnemy, float damage)
    {
        Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, playerStats.move);

        //if ()
        {

        }
    }

    public void Wack(Vector3Int hexCoordOfEnemy, float damage)
    {
        Pathfinding.AllPossibleTiles(hexObjInfo.hexCoord, 1);

       // if ()
        {

        }
    }

    public void Move(Vector3Int hexCoodOfEnemy, float damge)
    {
        
    }

    /*
WacK / Range(Vector3Int hexCoordOfEnemy, damage){
    get list of all tiles in range  //Using Pathfinding.getAllTiles()
    
    if( selected tile has enemey)  // Can use GlobalaVars list of eneimes
        damage enemy
    

}
*/

    /*
Move (Vector3Int hexCoordOfEnemy, range){
    get list of all tiles in range  //Using Pathfinding.getAllTiles()
    
    if( Check if tile is in in range)  // Can use GlobalaVars list of ene
        move to target Hex
}
*/

}

