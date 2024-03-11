using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars {
    //Map Info
    public static Dictionary<Vector3Int , GameObject> hexagonTile = new Dictionary<Vector3Int , GameObject>(); //Vector3Int - The Cubic Coord of Hex; Hex - The Hex Information
    public static Dictionary<Vector3Int , TileScriptableObjects> hexagonTileRefrence = new Dictionary<Vector3Int , TileScriptableObjects>(); //Vector3Int - The Cubic Coord of Hex; Hex - The Hex Information
    public static List<Vector3Int> availableHexes = new List<Vector3Int>(); //A list holding the coords of tile that exist on the map. 

    public static Vector3Int bottomLeftHex = new Vector3Int(0, 0 ,0);
    public static Vector3Int centerHex = new Vector3Int(); //Center of Map
    public static Vector3Int topRightHex;

    //Unit Info
    public static List<Stats> choosenPlayers = new List<Stats>();
    public static List<Stats> enemyStats = new List<Stats>();

    //Combat Info
    public static Dictionary<Vector3Int , Stats> players = new Dictionary<Vector3Int , Stats>();
    public static Dictionary<Vector3Int , Stats> enemies = new Dictionary<Vector3Int , Stats>();

    public static Queue<SpawnWave> spawnWaves = new Queue<SpawnWave>();


}
