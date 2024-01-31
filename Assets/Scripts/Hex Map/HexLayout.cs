using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexLayout : MonoBehaviour {
    public GameObject Testagon;
    public Vector2Int size;

    public Vector3Int offset2 = new Vector3Int(0, 0, 0);

    public GameObject tilemapObj;

    public List<TileScriptableObjects> tileTemplates = new List<TileScriptableObjects>();

    Vector2 spacing;

    private void Awake() {
        //Get Spacing Between Hexagons Based of Hexagon Size
        Bounds b = Testagon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
        spacing = new Vector2(b.size.x , 0.75f * b.size.y) * 2;

        //Generate Map
        UnityTileMap_To_HexGrid();
        Destroy(tilemapObj); //Don't need Unity's tile Map any longer
        Center();
    }

    public void UnityTileMap_To_HexGrid() {
        //Compress Bounds and Find Bounds of Tile Map
        Tilemap tilemap = tilemapObj.transform.GetChild(0).GetComponent<Tilemap>();
        tilemap.CompressBounds();
        BoundsInt tileMapBounds = tilemap.cellBounds;

        //Retrive All Unity Tiles and Convert them To My Hexagon Tiles
        TileBase[] allTiles = tilemap.GetTilesBlock(tileMapBounds);
        GameObject header = new GameObject();
        header.name = "header";

        for(int x = 0; x < tileMapBounds.size.x; x++) {
            for(int y = 0; y < tileMapBounds.size.y; y++) {

                //Create each tile (if it is not empty) and then added to the header
                TileBase tile = allTiles[x + y * tileMapBounds.size.x];
                if(tile != null) {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    GameObject tileObj = CreateHexTile(x , y , tile.name);
                    tileObj.transform.parent = header.transform;
                }
                else {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
    }

    private Vector3Int UnityCoords_To_CubicCoords(int y , int x) {
        var q = y - (x + 1) / 2; //var q = y - (x + (evenGrid & 1)) / 2;
        var r = x;
        return new Vector3Int(q , r , -q - r);
    }

    public GameObject CreateHexTile(int x, int y, string spriteName) {
        //Get Coordinate of Hexagon in Cubic Coordinates
        Hex h = new Hex(UnityCoords_To_CubicCoords(x, y));

        //Create Object, name it, and postion it. 
        GameObject obj = Instantiate(Testagon);
        obj.name = h.q + " " + h.r + " " + h.s;
        //obj.name = "(" + x.ToString() + ", " + y.ToString() + ")";
        //obj.name = spriteName;

        Vector2 pos = new Vector2(spacing.x * h.q , spacing.y * h.r) * 0.5f;
        Vector2 offset = new Vector2(h.r * spacing.x * 0.25f , 0);
        obj.transform.position = pos + offset;

        //Temporay Equations to put text on tiles
        //obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + x.ToString() + ", " + y.ToString() + ")";
        obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "";
        //obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = spriteName;


        obj.transform.GetComponent<HexObjInfo>().hexCoord = new Vector3Int(h.q , h.r , h.s);

        //Add Hexagon Info to GlobalVars
        GlobalVars.availableHexes.Add(new Vector3Int(h.q , h.r , h.s));
        GlobalVars.hexagonTile.Add(new Vector3Int(h.q , h.r , h.s) , obj);

        //Add Sprite to object and return obj
        TileScriptableObjects tileTemplate = GetTileTemplate(spriteName);
        if(tileTemplate == null)
            return obj;

        GlobalVars.hexagonTileRefrence.Add(new Vector3Int(h.q , h.r , h.s), tileTemplate);


        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tileTemplate.sprite;
        return obj;
    }

    public TileScriptableObjects GetTileTemplate(string spriteName) {
        //Get the TileObject the represents the sprite painted 
        for(int i = 0; i < tileTemplates.Count; i++) {
            //Debug.Log(tileTemplates[i].sprite.name);
            //Debug.Log(spriteName);
            if(tileTemplates[i].sprite.name == spriteName) {
                return tileTemplates[i];
            }
        }

        //Sprite Name Does not Exist in List of defined tiles
        Debug.LogWarning("Tile Sprite Doesn't Exist - " + spriteName);
        return null;
    }

    public void Center() {
        BoundsInt tileMapBounds = tilemapObj.transform.GetChild(0).GetComponent<Tilemap>().cellBounds;

        //Start in bottom left and go to the Top Right Corner
        Hex currentHex = new Hex(0 , 0 , 0);
        bool noNextHex = false;

        while(!noNextHex) {
            //Go Up Right
            if(GlobalVars.availableHexes.Contains(currentHex.getVector() + Hex.hex_directions[1])) {
                //GlobalVars.hexagonTile[currentHex.getVector()].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = 
                //    "(" + currentHex.q + ", " + currentHex.r + ", " + currentHex.s  + ")";
                currentHex = new Hex(currentHex.getVector() + Hex.hex_directions[1]);
                continue;
            }

            //Go Right
            if(GlobalVars.availableHexes.Contains(currentHex.getVector() + Hex.hex_directions[2])) {
                //GlobalVars.hexagonTile[currentHex.getVector()].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text =
                //    "(" + currentHex.q + ", " + currentHex.r + ", " + currentHex.s + ")";
                currentHex = new Hex(currentHex.getVector() + Hex.hex_directions[2]);
                continue;
            }

            //GlobalVars.hexagonTile[currentHex.getVector()].transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text =
            //        "(" + currentHex.q + ", " + currentHex.r + ", " + currentHex.s + ")";
            noNextHex = true;
        }

        Vector3Int centerTile = new Vector3Int(currentHex.q / 2, - (currentHex.q / 2) - (currentHex.s / 2) , currentHex.s / 2);
        Vector3 centerPos = GlobalVars.hexagonTile[centerTile].transform.position;
        Debug.Log("Ceneter Tile: " + centerTile);
        Debug.Log("Offset: " + centerPos);

        foreach(var t in GlobalVars.hexagonTile) {
            t.Value.transform.position -= centerPos;
        }

        GlobalVars.centerHex = centerTile;
    }
}
