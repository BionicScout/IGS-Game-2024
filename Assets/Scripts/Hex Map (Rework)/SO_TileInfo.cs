using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XXXTile" , menuName = "ScriptableObjects/Tile (NEW)" , order = 1)]

public class SO_TileInfo : ScriptableObject {
    public bool isPassable = true;
    public bool isInteractable = false;
    public Sprite sprite; 

    [HideInInspector]
    public Sprite defaultHexSprite;
}
