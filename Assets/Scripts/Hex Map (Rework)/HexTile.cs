using UnityEngine;

public class HexTile : MonoBehaviour {
    public int q, r, s;
    public bool isPassable = true;
    public bool isInteractable = false;
    public SpriteRenderer[] spriteLayers; //Layers from bottom to top: tile base, background decoration, object, forground decoration, highlight
    public Sprite defaultHexSprite;

    /*********************************
        Constructors   (Maybe Switch out for Set Methods)
    *********************************/

    // Cube Coord Constructor
    public void setHexTile(int x , int y , int z) {
        if(x + y + z != 0) {
            Debug.LogError("Cube coordinates do not sum to 0");
        }
        q = x;
        r = y;
        s = z;
        Initialize();
    }

    // Cube Coord Constructor using Vector3Int
    public void setHexTile(Vector3Int coords) {
        if(coords.x + coords.y + coords.z != 0) {
            Debug.LogError("Cube coordinates do not sum to 0");
        }
        q = coords.x;
        r = coords.y;
        s = coords.z;
        Initialize();
    }

    // Axial Coord Constructor
    public void setHexTile(int x , int y) {
        q = x;
        r = y;
        s = -x - y;
        Initialize();
    }

    // Initialization method
    public void Initialize() {
        InitializeSpriteLayers(5);
        SetSpriteLayer(0 , defaultHexSprite);
        this.gameObject.name = "(" + q + ", " + r + ", " + s + ")";
    }

    /*********************************
        SpriteRender and Sprite Layers
    *********************************/

    public void InitializeSpriteLayers(int layerCount) {
        spriteLayers = new SpriteRenderer[layerCount];
        for(int i = 0; i < layerCount; i++) {
            GameObject layerObject = new GameObject("Layer" + i);
            layerObject.transform.SetParent(this.transform , false);
            spriteLayers[i] = layerObject.AddComponent<SpriteRenderer>();
            spriteLayers[i].sortingOrder = i; // Ensures correct rendering order
        }
    }

    public void SetSpriteLayer(int layer , Sprite sprite) {
        if(layer < spriteLayers.Length) {
            spriteLayers[layer].sprite = sprite;
        }
        else {
            Debug.LogWarning("Layer index out of range");
        }
    }

    public Sprite GetSpriteLayer(int layer) {
        if(layer < spriteLayers.Length) {
            return spriteLayers[layer].sprite;
        }
        Debug.LogWarning("Layer index out of range");
        return null;
    }

    /*********************************
        Utiliy
    *********************************/

    public Vector3Int getCoords() {
        return new Vector3Int(q , r , s);
    }
}
