using System.Collections.Generic;
using UnityEngine;

public class HexGridTest : MonoBehaviour {
    private HexGrid hexGrid;
    public Sprite tileSprite;

    void Start() {
        hexGrid = GetComponent<HexGrid>();

        foreach (KeyValuePair<Vector3Int, HexTile> tileInfo in hexGrid.hexTiles) {
            tileInfo.Value.SetSpriteLayer(0, tileSprite);
        }


        // Test getting neighbors for a tile at (0, 0, 0)
        Vector3Int testCoords = new Vector3Int(2 , 1 , -3);
        if(hexGrid.hexTiles.ContainsKey(testCoords)) {
            HexTile testTile = hexGrid.hexTiles[testCoords];
            List<HexTile> neighbors = hexGrid.GetNeighbors(testTile);

            foreach(var neighbor in neighbors) {
                Debug.Log($"Neighbor of ({testTile.q}, {testTile.r}, {testTile.s}): ({neighbor.q}, {neighbor.r}, {neighbor.s})");
            }
        }
    }
}
