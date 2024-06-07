using UnityEngine;

public class HexTileExample : MonoBehaviour {
    public HexTile hexTilePrefab;
    public Sprite tileBaseSprite;
    public Sprite backgroundDecorationSprite;
    public Sprite objectSprite;
    public Sprite foregroundDecorationSprite;

    void Start() {
        // Create a new HexTile instance
        HexTile hexTile = Instantiate(hexTilePrefab , new Vector3(0 , 0 , 0) , Quaternion.identity);

        // Initialize the hex tile at a specific coordinate (using axial coordinates in this case)
        hexTile.Initialize(); // Ensure sprite layers are initialized

        // Set the sprites for each layer
        hexTile.SetSpriteLayer(0 , tileBaseSprite); // Base layer
        hexTile.SetSpriteLayer(1 , backgroundDecorationSprite); // Background decoration layer
        hexTile.SetSpriteLayer(2 , objectSprite); // Object layer
        hexTile.SetSpriteLayer(3 , foregroundDecorationSprite); // Foreground decoration layer
    }
}
