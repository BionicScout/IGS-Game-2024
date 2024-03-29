using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XXXTile" , menuName = "ScriptableObjects/Tile" , order = 1)]
public class TileScriptableObjects : ScriptableObject {
    public Sprite sprite;

    public bool isObstacle;
    public bool interactable;

    public TileScriptableObjects objToChange;
    public List<Vector3Int> tileChanges = new List<Vector3Int>();
}