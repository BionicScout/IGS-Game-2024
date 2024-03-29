using System;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour {
    public Vector3Int currentHex;
    public Vector3Int goalHex;

    public int range = 5;

    public Rigidbody2D rb;

    public Sprite playerSprite;

    private void Start() {
        currentHex = GlobalVars.centerHex;
        GlobalVars.hexagonTile[currentHex].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = 
            "(" + currentHex.x + ", " + currentHex.y + ", " + currentHex.z + ")";

        GlobalVars.hexagonTile[currentHex].transform.GetComponent<HexObjInfo>().unitName = "Player";
        GlobalVars.hexagonTile[currentHex].transform.GetComponent<HexObjInfo>().unitSprite = playerSprite;
        GlobalVars.hexagonTile[currentHex].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = playerSprite;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.BackQuote)) {
            //moveTile(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1)) {
            //moveTile(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) {
            //moveTile(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            //moveTile(3);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4)) {
            //oveTile(4);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5)) {
            //moveTile(5);
        }

        
        if(Input.GetKeyDown(KeyCode.Z)) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(currentHex , range)) {
                Vector3Int t = temp.Item1;
                //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + t.x + ", " + t.y + ", " + t.z + ")";
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        if(Input.GetKeyDown(KeyCode.X)) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(currentHex , range)) {
                Vector3Int t = temp.Item1;
                //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = temp.Item2.ToString();
            }
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(currentHex , range)) {
                Vector3Int t = temp.Item1;
                //GlobalVars.hexagonTile[t].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "";
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(false);
            }

            //GlobalVars.hexagonTile[currentHex].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text =
            //    "(" + currentHex.x + ", " + currentHex.y + ", " + currentHex.z + ")";
        }

        /*

        if(Input.GetKeyDown(KeyCode.I)) {
            goalHex = currentHex;
        }
        if(Input.GetKeyDown(KeyCode.O)) {
            int dist = 0;
            foreach(Vector3Int temp in Pathfinding.PathBetweenPoints(currentHex, goalHex)) {
                GlobalVars.hexagonTile[temp].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = dist.ToString();
                dist++;
            }

            GlobalVars.hexagonTile[currentHex].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "Start";
            GlobalVars.hexagonTile[goalHex].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "End";
        }
        if(Input.GetKeyDown(KeyCode.P)) {
            foreach(Vector3Int temp in Pathfinding.PathBetweenPoints(currentHex , goalHex)) {
                GlobalVars.hexagonTile[temp].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "";
            }

            GlobalVars.hexagonTile[currentHex].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text =
                "(" + currentHex.x + ", " + currentHex.y + ", " + currentHex.z + ")";
        }*/
    }

    //public void moveTile(int dir) {
    //    Vector3Int newHex = currentHex + Hex.hex_directions[dir];

    //    //Avoid Off Baord
    //    if(!GlobalVars.availableHexes.Contains(newHex)) {
    //        Debug.Log("Off Baord");
    //        return;
    //    }

    //    //Avoid Obsticale
    //    TileScriptableObjects template = GlobalVars.hexagonTileRefrence[newHex];
    //    if(template.isObstacle) {
    //        Debug.Log("Obsticale - " + template.name);
    //        return;
    //    }


    //    //Adjust Postion
    //    GameObject obj = GlobalVars.hexagonTile[currentHex];
    //    obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "";
    //    HexObjInfo tileUnit = obj.transform.GetComponent<HexObjInfo>();

    //    string unitName = tileUnit.name;
    //    Sprite sprite = tileUnit.unitSprite;

    //    tileUnit.unitSprite = null;
    //    tileUnit.unitName = "";
    //    obj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;

    //    currentHex = newHex;

    //    GlobalVars.hexagonTile[currentHex].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text =
    //    "(" + currentHex.x + ", " + currentHex.y + ", " + currentHex.z + ")";

    //    HexObjInfo newTileUnit = GlobalVars.hexagonTile[currentHex].transform.GetComponent<HexObjInfo>();
    //    newTileUnit.unitName = unitName;
    //    newTileUnit.unitSprite = sprite;
    //    GlobalVars.hexagonTile[currentHex].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = sprite;
    //}

    public static void movePlayer(Vector3Int playerCoord, Vector3Int newTileCoord)
    {
        //Get Player and current + future hex objs
        Stats playerStats = GlobalVars.players[playerCoord];
        GameObject currentTileObj = GlobalVars.hexagonTile[playerCoord];
        GameObject newTileObj = GlobalVars.hexagonTile[newTileCoord];

        //Update Sprite
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
        newTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = playerStats.sprite;

        //Update player coord
        GlobalVars.players.Remove(playerCoord);
        GlobalVars.players.Add(newTileCoord, playerStats);


        Vector3 offset = newTileObj.transform.position - currentTileObj.transform.position;
        //playerStats.healthBar.transform.position = playerStats.healthBar.transform.position + offset;
    }

    public static void moveEnemy(Vector3Int enemyCoord , Vector3Int newTileCoord) {
        //Get Player and current + future hex objs
        Stats enemyStats = GlobalVars.enemies[enemyCoord];
        GameObject currentTileObj = GlobalVars.hexagonTile[enemyCoord];
        GameObject newTileObj = GlobalVars.hexagonTile[newTileCoord];

        //Update Sprite
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
        newTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = enemyStats.sprite;

        //Update player coord
        GlobalVars.enemies.Add(newTileCoord , enemyStats);
        GlobalVars.enemies.Remove(enemyCoord);

    }
}
