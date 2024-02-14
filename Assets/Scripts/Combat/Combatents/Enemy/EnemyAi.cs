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
    int playerHitPenaltyWeight = 6;
    int farPlayerPenalty = -100;
    int distanceThreshold = 10;

    //Testing Purposes
    List<Vector3Int> debugMoveTiles = new List<Vector3Int>();
    Dictionary<Vector3Int, int> playersCanHitList = new Dictionary<Vector3Int, int>();

    void Start() {
        foreach(KeyValuePair<Vector3Int, Stats> info in GlobalVars.enemies) {
            enemyCoords.Add(info.Key);
        }

        Debug.Log("Enemy Count: " + enemyCoords.Count);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Enemy " + currentEnemyIndex + ": " + enemyCoords[currentEnemyIndex].ToString());

            //Clear Debug
            foreach(Vector3Int t in debugMoveTiles) {
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(false);
            }


            //Get Scores
            Stats stats = GlobalVars.enemies[enemyCoords[currentEnemyIndex]];

            List<KeyValuePair<Vector3Int , float>> tilesAndScore = ScoreTiles(stats);
            WriteToFile(tilesAndScore);

            //Move Ai
            Move(tilesAndScore);



            currentEnemyIndex = (currentEnemyIndex + 1) % enemyCoords.Count;
        }
    }

    public List<KeyValuePair<Vector3Int , float>> getTiles(Stats enemyStats) {

        List<KeyValuePair<Vector3Int, float>> tilesAndScores = new List<KeyValuePair<Vector3Int, float>>(); //Hex Coord, score

        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , enemyStats.move)) {
            tilesAndScores.Add(new KeyValuePair<Vector3Int, float>(info.Item1, 0));
        }

        return tilesAndScores;
    }

    /*********************************
        Scoring
    *********************************/

    List<KeyValuePair<Vector3Int , float>> ScoreTiles(Stats enemyStats) {
        List<KeyValuePair<Vector3Int, float>> tilesAndScores = getTiles(enemyStats);

        //Variables that are reused
        float score;
        List<int> distances;
        int closestPerson;
        int playersCanHit;

        playersCanHitList = new Dictionary<Vector3Int , int>();

        for(int tileScoreIndex = 0; tileScoreIndex < tilesAndScores.Count; tileScoreIndex++) {
            // Set tile Score
            score = 0;

            if(GlobalVars.players.ContainsKey(tilesAndScores[tileScoreIndex].Key)) {
                KeyValuePair<Vector3Int , float> t = tilesAndScores[tileScoreIndex];
                tilesAndScores[tileScoreIndex] = new KeyValuePair<Vector3Int , float>(tilesAndScores[tileScoreIndex].Key , float.MinValue);
                continue;
            }

            //Find PLayer Distances Info
            distances = new List<int>();
            closestPerson = -1;
            playersCanHit = 0;

            Debug.Log("--------" + tilesAndScores[tileScoreIndex].Key + "-------------");
            foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players){
                int distance = Pathfinding.PathBetweenPoints(playerInfo.Key , tilesAndScores[tileScoreIndex].Key).Count - 1;
                Debug.Log("Distance: " + distance);
                distances.Add(distance);

                if (closestPerson < distance) {
                    closestPerson = distance;
                }

                if (distance <= playerInfo.Value.attackRange) {
                    Debug.Log("Can Hit - " + distance + " <= " + playerInfo.Value.attackRange);
                    playersCanHit++;
                }

                Debug.Log("Can Hit 2 - " + playersCanHit);
            }

            playersCanHitList.Add(tilesAndScores[tileScoreIndex].Key, playersCanHit);

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
        Actions
    *********************************/
    private void Move(List<KeyValuePair<Vector3Int , float>> tilesAndScore) {
        //Get Max Value
        float maxValue = float.MinValue;

        foreach(KeyValuePair<Vector3Int , float> pair in tilesAndScore) {
            if(pair.Value > maxValue) {
                maxValue = pair.Value;
            }
        }

        //Get List maxValue
        List<Vector3Int> tiles = new List<Vector3Int>();

        foreach(KeyValuePair<Vector3Int , float> pair in tilesAndScore) {
            if(pair.Value == maxValue) {
                tiles.Add(pair.Key);
            }
        }

        //Pick one of the moves
        int randomMove = UnityEngine.Random.Range(0, tiles.Count);
        Vector3Int moveTile = tiles[randomMove];

        Debug.Log(tiles.Count);

        //Debug
        debugMoveTiles = tiles;

        foreach(Vector3Int t in tiles) {
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(true);
        }
        //GlobalVars.hexagonTile[].transform.GetChild(4).gameObject.SetActive(true);
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
                if(tile.Value <= float.MinValue + 1)
                    writer.WriteLine(tile.Key + " - Score: " + tile.Value);
                else
                    writer.WriteLine(tile.Key + " - Score: " + tile.Value);
            }
            

            writer.Close();
            Debug.Log("Data written to file successfully!");
        }
    }
}

/*


//Predefined Weights
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
