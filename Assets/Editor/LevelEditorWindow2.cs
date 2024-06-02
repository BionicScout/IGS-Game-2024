using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditorWindow2 : EditorWindow {
    private Tilemap tilemap;
    private TileBase selectedTile;
    private Vector2 scrollPos;
    private Vector2Int gridSize = new Vector2Int(10 , 10); // Adjust grid size as needed
    private float hexRadius = 120f; // Adjust hex radius as needed

    [MenuItem("Window/Level Editor 2")]
    public static void ShowWindow() {
        GetWindow<LevelEditorWindow2>("Level Editor 2");
    }

    private void OnGUI() {
        GUILayout.Label("Level Editor 2" , EditorStyles.boldLabel);

        // Tilemap field
        tilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap" , tilemap , typeof(Tilemap) , true);

        // Selected Tile field
        selectedTile = (TileBase)EditorGUILayout.ObjectField("Selected Tile" , selectedTile , typeof(TileBase) , false);

        // Scrollable area for custom level editor UI elements
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos , GUILayout.Width(position.width) , GUILayout.Height(position.height - 100));

        DrawHexTilemap();

        EditorGUILayout.EndScrollView();

        // Mouse event handling for tile placement
        HandleMouseEvents();
    }

    private void DrawHexTilemap() {
        if(tilemap == null)
            return;

        for(int x = 0; x < gridSize.x; x++) {
            for(int y = 0; y < gridSize.y; y++) {
                Vector3Int cellPosition = new Vector3Int(x , y , 0);
                Vector3 worldPos = tilemap.CellToWorld(cellPosition);
                Vector2 guiPos = HandleUtility.WorldToGUIPoint(worldPos);

                Rect cellRect = new Rect(guiPos.x - hexRadius , guiPos.y - hexRadius , hexRadius * 2 , hexRadius * 2 * Mathf.Sqrt(3) / 2);
                Handles.DrawSolidRectangleWithOutline(cellRect , new Color(1 , 1 , 1 , 0.1f) , Color.black);

                TileBase tile = tilemap.GetTile(cellPosition);
                if(tile != null) {
                    Texture2D texture = AssetPreview.GetAssetPreview(tile);
                    if(texture != null) {
                        GUI.DrawTexture(cellRect , texture);
                    }
                }
            }
        }
    }

    private void HandleMouseEvents() {
        if(tilemap == null || selectedTile == null)
            return;

        Event e = Event.current;
        if(e.type == EventType.MouseDown && e.button == 0) {
            Vector2 mousePos = e.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            Vector3 worldPos = ray.origin;

            Vector3Int cellPosition = tilemap.WorldToCell(worldPos);
            tilemap.SetTile(cellPosition , selectedTile);
            e.Use();
        }
    }
}
