using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class HexGrid : MonoBehaviour {
    public GameObject hexTilePrefab;
    public Sprite exampleSpriteSize;

    public int gridWidth; // Number of columns
    public int gridHeight; // Number of rows

    private float hexWidth, hexHeight;


    public Dictionary<Vector3Int , HexTile> hexTiles = new Dictionary<Vector3Int , HexTile>();

    void Start() {
        hexWidth = exampleSpriteSize.bounds.size.x;
        hexHeight = exampleSpriteSize.bounds.size.y;

        GenerateGrid();
    }

    // Generate the hex grid
    private void GenerateGrid() {
        for(int col = 0; col < gridWidth; col++) {
            for(int row = 0; row < gridHeight; row++) {
                CreateHexTile(row , col);
            }
        }
    }

    private void CreateHexTile(int row, int col) {
        // Calculate the hex coordinates
        int q = col - (row / 2);
        int r = row;
        int s = -q - r;

        Vector3Int hexCoords = new Vector3Int(q , r , s);
        Vector3Int offsetCoord = new Vector3Int(col , row);
        Vector3 pos = HexOffsetToWorldCoordinates(offsetCoord);
        HexTile hexTile = CreateHexTile(pos , hexCoords);
        hexTiles.Add(hexCoords , hexTile);
    }



    // Create a hex tile at a given position and coordinates
    private HexTile CreateHexTile(Vector3 position , Vector3Int coords) {
        GameObject hexTileObject = Instantiate(hexTilePrefab , position , Quaternion.identity , this.transform);
        HexTile hexTile = hexTileObject.GetComponent<HexTile>();

        hexTile.setHexTile(coords);

        return hexTile;
    }

    // Convert hex coordinates to world coordinates
    public Vector3 HexOffsetToWorldCoordinates(Vector3Int hexCoords) {
        // Calculate x position
        float x = hexCoords.x * hexWidth;

        // Calculate y position and apply stagger for odd columns
        float y = hexCoords.y * hexHeight * 0.75f;
        if(hexCoords.y % 2 != 0) {
            x += hexWidth * 0.5f; // Offset by half the vertical distance between rows
        }
         
        return new Vector3(x , y , 0);
    }

    // Get neighbors of a hex tile
    public HexTile[] GetNeighbors(HexTile tile) {
        List<HexTile> neighbors = new List<HexTile>();
        List<Vector3Int> hex_directions = new List<Vector3Int> {
            new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1)
        };

        Vector3Int currentCoords = new Vector3Int(tile.q , tile.r , tile.s);
        foreach(var direction in hex_directions) {
            Vector3Int neighborCoords = currentCoords + direction;
            if(hexTiles.ContainsKey(neighborCoords)) {
                neighbors.Add(hexTiles[neighborCoords]);
            }
        }

        return neighbors.ToArray();
    }








    // Add a column to the grid
    public void AddColumn() {
        int col = gridWidth;
        for(int row = 0; row < gridHeight; row++) {
            CreateHexTile(col , row);
        }
        gridWidth++;
    }

    // Remove the last column from the grid
    public void RemoveColumn() {
        if(gridWidth <= 0)
            return;
        int col = gridWidth - 1;
        for(int row = 0; row < gridHeight; row++) {
            int q = col - (row / 2);
            int r = row;
            int s = -q - r;
            Vector3Int hexCoords = new Vector3Int(q , r , s);
            if(hexTiles.TryGetValue(hexCoords , out HexTile tile)) {
                Destroy(tile.gameObject);
                hexTiles.Remove(hexCoords);
            }
        }
        gridWidth--;
    }

    // Add a row to the grid
    public void AddRow() {
        int row = gridHeight;
        for(int col = 0; col < gridWidth; col++) {
            CreateHexTile(col , row);
        }
        gridHeight++;
    }

    // Remove the last row from the grid
    public void RemoveRow() {
        if(gridHeight <= 0)
            return;
        int row = gridHeight - 1;
        for(int col = 0; col < gridWidth; col++) {
            int q = col - (row / 2);
            int r = row;
            int s = -q - r;
            Vector3Int hexCoords = new Vector3Int(q , r , s);
            if(hexTiles.TryGetValue(hexCoords , out HexTile tile)) {
                Destroy(tile.gameObject);
                hexTiles.Remove(hexCoords);
            }
        }
        gridHeight--;
    }




    public void Update() {
        if(Input.GetKeyDown(KeyCode.Equals)) {
            AddRow();
        }
        if(Input.GetKeyDown(KeyCode.Minus)) {
            RemoveRow();
        }
        if(Input.GetKeyDown(KeyCode.RightBracket)) {
            AddColumn();
        }
        if(Input.GetKeyDown(KeyCode.LeftBracket)) {
            RemoveColumn();
        }
    }
}
