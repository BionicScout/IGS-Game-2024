using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class EnemyAi : MonoBehaviour {

    string filePath = "Assets\\/Scripts\\Combat\\Combatents\\Enemy\\EnemyAiDebug.txt"; // Define your file path here



    List<Vector3Int> enemyCoords = new List<Vector3Int>();
    int currentEnemyIndex = 0;

    void Start() {
        foreach(KeyValuePair<Vector3Int, Stats> info in GlobalVars.enemies) {
            enemyCoords.Add(info.Key);
        }

        Debug.Log("Enemy Count: " + enemyCoords.Count);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Enemy " + currentEnemyIndex + ": " + enemyCoords[currentEnemyIndex].ToString());

            List<KeyValuePair<Vector3Int , int>> tilesAndScore = getTiles();
            tilesAndScore = Score_AttackRange(tilesAndScore);
            WriteToFile(tilesAndScore);

            currentEnemyIndex = (currentEnemyIndex + 1) % enemyCoords.Count;
        }
    }

    public List<KeyValuePair<Vector3Int , int>> getTiles() {

        List<KeyValuePair<Vector3Int, int>> tilesAndScores = new List<KeyValuePair<Vector3Int, int>>(); //Hex Coord, score

        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , 3)) {
            tilesAndScores.Add(new KeyValuePair<Vector3Int, int>(info.Item1, 0));
        }

        return tilesAndScores;
    }


    public List<KeyValuePair<Vector3Int , int>> Score_AttackRange(List<KeyValuePair<Vector3Int , int>> tilesAndScores) {

        foreach(KeyValuePair<Vector3Int , int> tile in tilesAndScores) {
            //tile.Value;
            //Stats
            
        }


        return tilesAndScores;
    }





    void WriteToFile(List<KeyValuePair<Vector3Int , int>> tilesAndScores) {
        // Check if the file already exists, if not, create it
        if(!File.Exists(filePath)) {
            File.Create(filePath).Close();
        }

        // Open the file to write
        using(StreamWriter writer = new StreamWriter(filePath)) {
            // Write class information to the file
            writer.WriteLine("Tile Scores: ");

            foreach(KeyValuePair<Vector3Int, int> tile in tilesAndScores) {
                writer.WriteLine(tile.Key + " - Score: " + tile.Value);
            }
            

            writer.Close();
            Debug.Log("Data written to file successfully!");
        }
    }
}
