using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyAi : MonoBehaviour {

    string filePath = "Assets\\/Scripts\\Combat\\Combatents\\Enemy\\EnemyAiDebug.txt"; // Define your file path here



    List<Vector3Int> enemyCoords = new List<Vector3Int>();
    int currentEnemyIndex = 0;


    // Predefined Weights
    int inRangeWeight = 5;
    int playerHitPenaltyWeight = 2;
    int farPlayerPenalty = -100;
    int distanceThreshold = 10;

    void Start() {
        foreach(KeyValuePair<Vector3Int, Stats> info in GlobalVars.enemies) {
            enemyCoords.Add(info.Key);
        }

        Debug.Log("Enemy Count: " + enemyCoords.Count);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Enemy " + currentEnemyIndex + ": " + enemyCoords[currentEnemyIndex].ToString());

            Stats stats = GlobalVars.enemies[enemyCoords[currentEnemyIndex]];

            List<KeyValuePair<Vector3Int , float>> tilesAndScore = ScoreTiles();
            WriteToFile(tilesAndScore);

            currentEnemyIndex = (currentEnemyIndex + 1) % enemyCoords.Count;
        }
    }

    public List<KeyValuePair<Vector3Int , float>> getTiles() {

        List<KeyValuePair<Vector3Int, float>> tilesAndScores = new List<KeyValuePair<Vector3Int, float>>(); //Hex Coord, score

        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , 3)) {
            tilesAndScores.Add(new KeyValuePair<Vector3Int, float>(info.Item1, 0));
        }

        return tilesAndScores;
    }

    /*********************************
        Scoring
    *********************************/

    public List<KeyValuePair<Vector3Int , float>> ScoreTiles() {
        List<KeyValuePair<Vector3Int, float>> tilesAndScores = getTiles();

        //Variables that are reused
        float score;
        List<int> distances;
        int closestPerson;
        int playersCanHit;

        for(int tileScoreIndex = 0; tileScoreIndex < tilesAndScores.Count; tileScoreIndex++) {
            // Set tile Score
            score = 0;

            //Find PLayer Distances Info
            distances = new List<int>();
            closestPerson = -1;
            playersCanHit = 0;

            Debug.Log("--------New Tile-------------");
            foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players){
                int distance = Pathfinding.PathBetweenPoints(playerInfo.Key , tilesAndScores[tileScoreIndex].Key).Count + 1;
                Debug.Log("Distance: " + distance);
                distances.Add(distance);

                if (closestPerson < distance) {
                    closestPerson = distance;
                }

                if (distance <= playerInfo.Value.attackRange) {
                    playersCanHit++;
                }
            }

            Debug.Log("ClosestPerson: " + closestPerson);

            //Scoring
            foreach(int distance in distances){
                if(distance <= GlobalVars.enemies[enemyCoords[currentEnemyIndex]].attackRange)
                    score += (GlobalVars.enemies[enemyCoords[currentEnemyIndex]].attackRange / (float)distance) * inRangeWeight;
            }

            score -= playersCanHit * (playersCanHit + 1) * playerHitPenaltyWeight;

            if (closestPerson > distanceThreshold || closestPerson == -1) {
                score += farPlayerPenalty;
            }

            KeyValuePair<Vector3Int, float> temp = tilesAndScores[tileScoreIndex];
            tilesAndScores[tileScoreIndex] = new KeyValuePair<Vector3Int , float>(temp.Key, score);
        }

        return tilesAndScores;
    }


    /*********************************
        Debugging
    *********************************/

    void WriteToFile(List<KeyValuePair<Vector3Int , float>> tilesAndScores) {
        // Check if the file already exists, if not, create it
        if(!File.Exists(filePath)) {
            File.Create(filePath).Close();
        }

        // Open the file to write
        using(StreamWriter writer = new StreamWriter(filePath)) {
            // Write class information to the file
            writer.WriteLine("Tile Scores: ");

            foreach(KeyValuePair<Vector3Int, float> tile in tilesAndScores) {
                writer.WriteLine(tile.Key + " - Score: " + tile.Value);
            }
            

            writer.Close();
            Debug.Log("Data written to file successfully!");
        }
    }
}

/*


// Predefined Weights
inRangeWeight = 5
playerHitPenaltyWeight = 2
farPlayerPenalty = -100
distanceThreshold = 30

// Set tile Score
score = 0

//Find PLayer Distances Info
List<int> distances = new List<int>();
colsestPerson = -1
playersCanHit = 0
foreach player on board{
    distance = distance between ai and player
    distances.add(distance);

    if (closestPerson > distance) {
        closestPerson = distance
    }

    if (Player can hit tile) {
        playersCanHit++
    }
}

//Scoring
foreach(distance in distances){
    score += (Enemy.AttackRange / distance) * inRangeWeight
}

score -= playersCanHit * (playersCanHit + 1) * playerHitPenaltyWeight

if (closestPerson > distanceThreshold) {
    score += farPlayerPenalty
}







*/
