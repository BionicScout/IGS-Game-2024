using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XXXTile" , menuName = "ScriptableObjects/Tile" , order = 1)]
public class TileScriptableObjects : ScriptableObject {
    public Sprite sprite;

    public bool isObstacle; 
}