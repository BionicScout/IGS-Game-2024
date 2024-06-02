using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class LevelEditorWindow : EditorWindow {
    private Tilemap tilemap;
    private TileBase[] tiles;
    private Grid grid;
    private Vector2 scrollPos;
    private TileBase selectedTile;
    private Vector2Int mapSize = new Vector2Int(1 , 5); // Adjust the map size as needed

    [MenuItem("Window/Level Editor")]
    public static void ShowWindow() {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnEnable() {
        // Initialize the tilemap and tiles array
        // Load all available tiles (assuming they are located in a specific folder)
        string tilePath = "Assets/Tiles"; // Adjust path as needed
        tiles = AssetDatabase.LoadAllAssetsAtPath(tilePath).OfType<TileBase>().ToArray();

        // Initialize the grid and tilemap
        GameObject gridGameObject = new GameObject("EditorGrid");
        grid = gridGameObject.AddComponent<Grid>();
        grid.cellLayout = GridLayout.CellLayout.Hexagon;
        grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;

        GameObject tilemapGameObject = new GameObject("EditorTilemap");
        tilemap = tilemapGameObject.AddComponent<Tilemap>();
        tilemapGameObject.AddComponent<TilemapRenderer>();
        tilemap.transform.SetParent(grid.transform);
    }

    private void OnDisable() {
        // Clean up the tilemap to avoid cluttering the scene
        if(tilemap != null) {
            DestroyImmediate(tilemap.gameObject);
        }
    }

    private void OnGUI() {
        // Begin horizontal layout
        GUILayout.BeginHorizontal();

        // Left side: Tile Map
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.6f));
        GUILayout.Label("Tile Map" , EditorStyles.boldLabel);

        // Tilemap display area
        DisplayTileMap();
        GUILayout.EndVertical();

        // Right side: List of Scriptable Objects
        GUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f));
        GUILayout.Label("Scriptable Objects" , EditorStyles.boldLabel);

        // Placeholder for Scriptable Objects grid
        GUILayout.Box("Scriptable Objects Area" , GUILayout.ExpandHeight(true) , GUILayout.ExpandWidth(true));
        GUILayout.EndVertical();

        // End horizontal layout
        GUILayout.EndHorizontal();
    }

    private void DisplayTileMap() {
        // Placeholder for displaying and interacting with the tile map
        Rect tileMapRect = GUILayoutUtility.GetRect(0 , 0 , GUILayout.ExpandWidth(true) , GUILayout.ExpandHeight(true));
        GUI.Box(tileMapRect , "Tile Map Area");

        // Load the hexagonal sprite
        Texture2D hexSprite = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ART _ TILES/Tiles and Pallets/Test Tiles/Tiles/PathTile.png");

        // Ensure the sprite is loaded
        if(hexSprite == null) {
            Debug.LogError("Hex sprite not found at the specified path.");
            return;
        }

        // Draw the grid with sprites
        float hexWidth = hexSprite.width;
        float hexHeight = hexSprite.height;
        float hexVerticalSpacing = hexHeight; // Vertical spacing for pointed-top hexagons
        float hexHorizontalSpacing = hexWidth; // Horizontal spacing for pointed-top hexagons

        for(int y = 0; y < mapSize.y; y++) {
            for(int x = 0; x < mapSize.x; x++) {
                float xPos = tileMapRect.x + x * hexHorizontalSpacing * 0.5f + 0.5f; // Horizontal offset by 75% of hexWidth
                float yPos = tileMapRect.y + y * hexVerticalSpacing * 0.75f; // Vertical offset every other column

                Rect hexRect = new Rect(xPos , yPos , hexWidth , hexHeight);
                GUI.DrawTexture(hexRect , hexSprite);
            }
        }
    }





}
