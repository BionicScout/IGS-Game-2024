using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars {
    public static Dictionary<Vector3Int, GameObject> hexagonTile = new Dictionary<Vector3Int, GameObject>(); //Vector3Int - The Cubic Coord of Hex; Hex - The Hex Information
    public static List<Vector3Int> availableHexes = new List<Vector3Int>(); //A list holding the coords of tile that exist on the map. 
}
