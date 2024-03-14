using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAi {
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

    public async Task enemyTurn(TurnManager tm) {

        //Debug.Log("------------ Enemy Turn ------------");

        foreach(KeyValuePair<Vector3Int , Stats> info in GlobalVars.enemies) {
            enemyCoords.Add(info.Key);
        }

        //Debug.Log("Enemy Count: " + enemyCoords.Count);

        List<Vector3Int> temp = new List<Vector3Int>(enemyCoords);

        currentEnemyIndex = 0;
        foreach(Vector3Int coord in temp) {
            Stats stats = GlobalVars.enemies[coord];

            if(stats.charType == "L1") {
                //Debug.Log("Level 1 Ai");
                tm.commandQueue.Enqueue(LevelOneAi(coord));
            }
            else if(stats.charType == "L2") {
                //Debug.Log("Level 2 Ai");
                tm.commandQueue.Enqueue(LevelTwoAi(coord));
            }
            else if(stats.charType == "L3") {
                //Debug.Log("Level 2 Ai");
                tm.commandQueue.Enqueue(LevelTwoAi(coord));
            }
            else {
                //Debug.Log("General Ai");
                tm.commandQueue.Enqueue(GeneralAi(coord));                
            }

            await Task.Yield();
            currentEnemyIndex++;

            //Debug.Log("Enemies Processed: " + currentEnemyIndex);
        }

        tm.startPlayerTurn();
        //FindObjectOfType<TurnManager>().EnemyturnTaken()
    }

    public List<KeyValuePair<Vector3Int , float>> getTiles(Stats enemyStats, Vector3Int currentEnemy) {

        List<KeyValuePair<Vector3Int, float>> tilesAndScores = new List<KeyValuePair<Vector3Int, float>>(); //Hex Coord, score

        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , enemyStats.move)) {
            if(GlobalVars.players.ContainsKey(info.Item1)) {
                continue;
            }

            if(enemyCoords.Contains(info.Item1) && info.Item1 != currentEnemy) {
                continue;
            }

            //Debug.Log(info.Item1);



            tilesAndScores.Add(new KeyValuePair<Vector3Int, float>(info.Item1, 0));
        }

        return tilesAndScores;
    }

    /*********************************
        Types of Ai
    *********************************/

    public Command GeneralAi(Vector3Int coord) {
        Stats stats = GlobalVars.enemies[coord];

        //Check if play is really far from player
        Vector3Int tileToClosestPLayer = playerIsfar(stats , coord);


        Command command;

        //If ClosestPlayer is (1, 1, 1) (which is an invalid tile) then score tiles, other wise moce to closestPLayer
        if(tileToClosestPLayer == Vector3Int.one) {
            //Debug.Log("CLOSE MOVE");
            //Get Scores
            List<KeyValuePair<Vector3Int , float>> tilesAndScore = GeneralScoreTiles(stats , coord);
            WriteToFile(tilesAndScore);

            //Move Ai
            command = Move(tilesAndScore);

            if(command.moveSpace == command.startSpace) {
                command.moveSpace = Vector3Int.one;
            }
        }
        else {
            //Debug.Log("FAR MOVE");
            command = new Command(coord , tileToClosestPLayer);
        }

        //Debug.Log("MOVE");

        //Attack Player
        //Debug.Log("ATTACK");
        command = Attack(command , stats);

        return command;
    }

    public Command LevelOneAi(Vector3Int coord) {
        Stats stats = GlobalVars.enemies[coord];

        //Check if play is really far from player
        Vector3Int tileToClosestPLayer = playerIsfar(stats , coord);
        
        Command command;

        //If ClosestPlayer is (1, 1, 1) (which is an invalid tile) then score tiles, other wise moce to closestPLayer
        if(tileToClosestPLayer == Vector3Int.one) {
            //Debug.Log("CLOSE MOVE");
            //Get Scores
            List<KeyValuePair<Vector3Int , float>> tilesAndScore = L1_ScoreTiles(stats , coord);
            //WriteToFile(tilesAndScore);

            //Move Ai
            command = Move(tilesAndScore);

            if(command.moveSpace == command.startSpace) {
                command.moveSpace = Vector3Int.one;
            }
        }
        else {
            //Debug.Log("FAR MOVE");
            command = new Command(coord , tileToClosestPLayer);
        }

        //Debug.Log("MOVE");

        //Attack Player
        //Debug.Log("ATTACK");
        command = Attack(command , stats);

        if(command.attackTile == Vector3Int.one)
            command = HouseAttack(command , stats);

        return command;
    }


    public Command LevelTwoAi(Vector3Int coord) {
        Stats stats = GlobalVars.enemies[coord];

        Command command = new Command(coord);
        //Debug.Log("ATTACK");
        command = Attack(command , stats);

        return command;
    }

    public Command LevelThreeAi(Vector3Int coord) {
        Stats stats = GlobalVars.enemies[coord];

        Command command = new Command(coord);
        //Debug.Log("ATTACK");
        command = Attack(command , stats);

        return command;
    }


    /*********************************
        Scoring
    *********************************/

    Vector3Int playerIsfar(Stats enemyStats, Vector3Int currentEnemy) {
        //Variables that are reused
        Vector3Int cloestPlayer = Vector3Int.one;
        int closestPerson = -1;

        foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
            int distance = Pathfinding.PathBetweenPoints(playerInfo.Key , currentEnemy).Count - 1;

            if(closestPerson < distance) {
                closestPerson = distance;
                cloestPlayer = playerInfo.Key; 
            }
        }

        if(closestPerson <= distanceThreshold) { 
            return Vector3Int.one;
        }


        Vector3Int closestTile = Vector3Int.one;
        int closestDistance = int.MaxValue;
        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , enemyStats.move)) {
            int distance = Pathfinding.PathBetweenPoints(cloestPlayer , info.Item1).Count - 1;

            if(distance < closestDistance) {
                closestDistance = distance;
                closestTile = info.Item1;
            }
        }

        return closestTile;
    }

    List<KeyValuePair<Vector3Int , float>> GeneralScoreTiles(Stats enemyStats, Vector3Int currentEnemy) {
        List<KeyValuePair<Vector3Int, float>> tilesAndScores = getTiles(enemyStats, currentEnemy);

        //Variables that are reused
        float score;
        List<int> distances;
        int closestPerson;
        int playersCanHit;

        playersCanHitList = new Dictionary<Vector3Int , int>();

        for(int tileScoreIndex = 0; tileScoreIndex < tilesAndScores.Count; tileScoreIndex++) {
            // Set tile Score
            score = 0;

            //Find PLayer Distances Info
            distances = new List<int>();
            closestPerson = -1;
            playersCanHit = 0;

            //Debug.Log("--------" + tilesAndScores[tileScoreIndex].Key + "-------------");
            foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players){
                int distance = Pathfinding.PathBetweenPoints(playerInfo.Key , tilesAndScores[tileScoreIndex].Key).Count - 1;
                //Debug.Log("Distance: " + distance);
                distances.Add(distance);

                if (closestPerson < distance) {
                    closestPerson = distance;
                }

                if (distance <= playerInfo.Value.attackRange) {
                    //Debug.Log("Can Hit - " + distance + " <= " + playerInfo.Value.attackRange);
                    playersCanHit++;
                }

                //Debug.Log("Can Hit 2 - " + playersCanHit);
            }

            playersCanHitList.Add(tilesAndScores[tileScoreIndex].Key, playersCanHit);

            //Debug.Log("ClosestPerson: " + closestPerson);

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

    List<KeyValuePair<Vector3Int , float>> L1_ScoreTiles(Stats enemyStats , Vector3Int currentEnemy) {
        List<KeyValuePair<Vector3Int , float>> tilesAndScores = getTiles(enemyStats , currentEnemy);

        //Variables that are reused
        float score;
        int closestHouse;
        int playersCanHit;

        playersCanHitList = new Dictionary<Vector3Int , int>();

        for(int tileScoreIndex = 0; tileScoreIndex < tilesAndScores.Count; tileScoreIndex++) {
            // Set tile Score
            score = 0;

            //House Scoring
            closestHouse = -1;
            foreach(Vector3Int houseInfo in GlobalVars.L1_houseTiles) {
                int distance = Pathfinding.PathBetweenPoints(houseInfo, tilesAndScores[tileScoreIndex].Key).Count - 1;

                if(closestHouse > distance || closestHouse == -1) {
                    closestHouse = distance;
                }
            }

            //Debug.Log(closestHouse + "\t" + tilesAndScores[tileScoreIndex].Key);
            if(closestHouse > enemyStats.attackRange)
                score += closestHouse * -20; 




            //Player Scoreing
            playersCanHit = 0;

            foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
                int distance = Pathfinding.PathBetweenPoints(playerInfo.Key , tilesAndScores[tileScoreIndex].Key).Count - 1;

                if(distance <= playerInfo.Value.attackRange) {
                    playersCanHit++;
                }
            }

            playersCanHitList.Add(tilesAndScores[tileScoreIndex].Key , playersCanHit);
            score -= playersCanHit * (playersCanHit + 1) * playerHitPenaltyWeight;

            //Save Tile Score
            KeyValuePair<Vector3Int , float> temp = tilesAndScores[tileScoreIndex];
            tilesAndScores[tileScoreIndex] = new KeyValuePair<Vector3Int , float>(temp.Key , score);
        }

        return tilesAndScores;
    }

    /*********************************
        Actions
    *********************************/
    private Command Move(List<KeyValuePair<Vector3Int , float>> tilesAndScore) {
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

        //Debug.Log(tiles.Count);

        //Debug
        debugMoveTiles = tiles;

        //foreach(Vector3Int t in tiles) {
        //    GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(true);
        //}
        //GlobalVars.hexagonTile[].transform.GetChild(4).gameObject.SetActive(true);


        Command command = new Command(enemyCoords[currentEnemyIndex] , moveTile);
        //Movement.moveEnemy(enemyCoords[currentEnemyIndex], moveTile);
        enemyCoords[currentEnemyIndex] = moveTile;
        return command;
    }

    private Command HouseAttack(Command command , Stats enemy) {
        command.attackTile = Vector3Int.one;

        foreach(Vector3Int houseTile in GlobalVars.L1_houseTiles) {
            //
            Vector3Int endTurnTile = command.moveSpace;
            if(endTurnTile != null) {
                endTurnTile = command.startSpace;
            }


            int distance = Pathfinding.PathBetweenPoints(houseTile , endTurnTile).Count - 1;

            if(distance == -1)
                continue;

            if(distance <= enemy.attackRange) {
                command.houseAttackTile = houseTile;
            }
        }

        //Debug.Log("END ATTACK");
        //Debug.Log("Attack Player at: " + playerCoord + "\nLowest Health: " + lowestHealth);

        return command;
    }

    private Command Attack(Command command, Stats enemy) {
        Vector3Int playerCoord = Vector3Int.one;
        float lowestHealth = float.MaxValue;

        //Debug.Log("--- Next Enemy ---");

        foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
            //
            Vector3Int endTurnTile = command.moveSpace;
            if(endTurnTile != null) {
                endTurnTile = command.startSpace;
            }
          

            int distance = Pathfinding.PathBetweenPoints(playerInfo.Key , endTurnTile).Count - 1;

            if(distance == -1)
                continue;

            //Debug.Log("DIST: " + distance);
            //Debug.Log("Distance: " + distance + "\nAttack Range: " + enemy.attackRange);
            //Debug.Log("Player Health: " + playerInfo.Value.curHealth + "\nLowest Health: " + lowestHealth);

            if(distance <= enemy.attackRange && playerInfo.Value.curHealth < lowestHealth) {
                lowestHealth = playerInfo.Value.curHealth;
                playerCoord = playerInfo.Key;
            }
        }

        //Debug.Log("END ATTACK");
        //Debug.Log("Attack Player at: " + playerCoord + "\nLowest Health: " + lowestHealth);

        command.attackTile = playerCoord;


        return command;
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