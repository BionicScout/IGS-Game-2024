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

    private void Start() {
        spacing = new Vector2(Mathf.Sqrt(3) * size.x , 1.5f * size.y);

        //TestGeneration();
        ImportTest();
    }

    public void ImportTest() {
        Tilemap tilemap = tilemapObj.transform.GetChild(0).GetComponent<Tilemap>();
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        TilemapRenderer tilemapRenderer = tilemapObj.transform.GetChild(0).GetComponent<TilemapRenderer>();


        //BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        GameObject header = new GameObject();
        header.name = "header";

        //Create Tiles
        for(int x = 0; x < bounds.size.x; x++) {
            for(int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile != null) {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    GameObject tileObj = CreateHexTile(x , y , tile.name);
                    tileObj.transform.parent = header.transform;
                }
                else {
                    //Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }

        Destroy(tilemapObj);


        //Adjust Center Postiton
        Vector3Int max = new Vector3Int();
        Vector3Int min = new Vector3Int();
        foreach(var t in GlobalVars.hexagonTile) {
            if(max.x > t.Key.x)
                max.x = t.Key.x;
            else if(min.x < t.Key.x)
                min.x = t.Key.x;

            if(max.y > t.Key.y)
                max.y = t.Key.y;
            else if(min.y < t.Key.y)
                min.y = t.Key.y;
        }

        Vector3Int centerTile = UnityCoords_To_CubicCoords(bounds.size.y / 2 , bounds.size.x / 2);
        //Vector3Int centerTile = (max + min) / 2;
        //centerTile.z = -centerTile.x - centerTile.y;
        Vector3 centerPos = GlobalVars.hexagonTile[centerTile].transform.position;
        Debug.Log("Ceneter Tile: " + centerTile);
        Debug.Log("Offset: " + centerPos);

        foreach(var t in GlobalVars.hexagonTile) {
            t.Value.transform.position -= centerPos;
        }

        //Set Move
        //GlobalVars.hexagonTile[centerTile].transform.position
    }

    private Vector3Int UnityCoords_To_CubicCoords(int y , int x) {
        var q = y - (x + 1) / 2; //var q = y - (x + (evenGrid & 1)) / 2;
        var r = x;
        return new Vector3Int(q , r , -q - r);
    }

    public GameObject CreateHexTile(int x, int y, string spriteName) {
        Hex h = new Hex(UnityCoords_To_CubicCoords(x, y));
        GlobalVars.availableHexes.Add(new Vector3Int(h.q , h.r , h.s));

        GameObject obj = Instantiate(Testagon);
        obj.name = h.q + " " + h.r + " " + h.s;
        //obj.name = "(" + x.ToString() + ", " + y.ToString() + ")";

        Vector2 pos = new Vector2(spacing.x * h.q , spacing.y * h.r) * 0.5f;
        Vector2 offset = new Vector2(h.r * spacing.x * 0.25f , 0);
        obj.transform.position = pos + offset;

        obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "(" + x.ToString() + ", " + y.ToString() + ")";
        //obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "";
        //obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = spriteName;

        GlobalVars.hexagonTile.Add(new Vector3Int(h.q , h.r , h.s) , obj);

        TileScriptableObjects tileTemplate = getTileTemplate(spriteName);
        if(tileTemplate == null)
            return obj;


        obj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tileTemplate.sprite;
        return obj;
    }

    public TileScriptableObjects getTileTemplate(string spriteName) {
        for(int i = 0; i < tileTemplates.Count; i++) {
            if(tileTemplates[i].sprite.name == spriteName) {
                return tileTemplates[i];
            }
        }

        //Debug.LogWarning("Tile Sprite Doesn't Exist - " + spriteName);
        return null;
    }

    public void TestGeneration() {
        Vector2 spacing = new Vector2(Mathf.Sqrt(3) * size.x , 1.5f * size.y);
        Hex center = new Hex(-1 , -1 , -1);

        //All Tiles
        for(int q = -12; q <= 12; q++) {
            for(int r = -12; r <= 12; r++) {
                Hex h = new Hex(q , r , -q - r);
                GlobalVars.availableHexes.Add(new Vector3Int(h.q , h.r , h.s));

                GameObject obj = Instantiate(Testagon);
                obj.name = q + " " + r + " " + (-q - r);

                Vector2 pos = new Vector2(spacing.x * q , spacing.y * r) * 0.5f;
                Vector2 offset = new Vector2(r * spacing.x * 0.25f , 0);
                obj.transform.position = pos + offset;

                obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "-1";

                GlobalVars.hexagonTile.Add(new Vector3Int(h.q , h.r , h.s) , obj);

                if(q == 0 && r == 0)
                    center = h;
            }
        }


        center += offset2;


        //Find All neighnors
        for(int i = 0; i < GlobalVars.availableHexes.Count; i++) {
            Vector3Int pos = GlobalVars.availableHexes[i];
            Hex h = new Hex(pos);
            GameObject obj = GlobalVars.hexagonTile[pos];

            string s = h.distance(center).ToString();
            obj.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = s;
        }
    }


}
