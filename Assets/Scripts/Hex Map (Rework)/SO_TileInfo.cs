using System.Collections.Generic;
using UnityEngine;

public class SO_TileInfo : ScriptableObject {
    public bool isPassable = true;
    public bool isInteractable = false;
    public SpriteRenderer[] spriteLayers; //Layers from bottom to top: tile base, background decoration, object, forground decoration, highlight

    [HideInInspector]
    public Sprite defaultHexSprite;
}
