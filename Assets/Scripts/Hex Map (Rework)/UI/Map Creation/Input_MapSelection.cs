using UnityEngine;

public class Input_MapSelection : MonoBehaviour
{
    public SO_TileInfo data;
    public HexGrid hexGrid;

    public void HandleTileClick(GameObject tileObj) {
        HexUIList hexUIList = tileObj.GetComponent<HexUIList>();  

        if(hexUIList != null) {
            // Example of accessing data
            Debug.Log("Tile data: " + hexUIList.data.name);
            data = hexUIList.data;
            // Call other methods on hexUIList if needed
        }
        else {
            Debug.LogWarning("HexUIList component not found on tile object.");
        }
    }

    void Update() {
        if(Input.GetMouseButton(0)) {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = -Camera.main.transform.position.z;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint);
            Vector2 gridPoint = new Vector2(worldPoint.x , worldPoint.y);
            Vector3Int hexCoords = hexGrid.WorldToCubic(gridPoint);
            if(hexGrid.hexTiles.ContainsKey(hexCoords)) {
                HexTile hexTile = hexGrid.hexTiles[hexCoords];
                hexTile.SetSpriteLayer(0, data.sprite);
            }

        }
    }

}
