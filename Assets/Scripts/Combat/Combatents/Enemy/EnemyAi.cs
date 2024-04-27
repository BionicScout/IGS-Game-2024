
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms.Impl;

public class EnemyAi {
    string filePath = "Assets\\/Scripts\\Combat\\Combatents\\Enemy\\EnemyAiDebug.txt"; // Define your file path here


    List<Vector3Int> enemyCoords = new List<Vector3Int>();
    List<Vector3Int> attackedHouses = new List<Vector3Int>();
    int currentEnemyIndex = 0;

    // Predefined Weights
    int inRangeWeight = 5;
    int playerHitPenaltyWeight = 6;
    int farPlayerPenalty = -100;
    int distanceThreshold = 7;

    //Testing Purposes
    List<Vector3Int> debugMoveTiles = new List<Vector3Int>();
    Dictionary<Vector3Int, int> playersCanHitList = new Dictionary<Vector3Int, int>();

    //Constants
    int MAX_ITERATIONS = 2000;

    //
    double totalTime = 0;
    int iterations = 0;

    public void enemyTurn(TurnManager tm) {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        totalTime = 0;
        iterations = 0;


        //Error Detection
        if(GlobalVars.enemies == null) {
            Debug.Log("ERROR - GlobalVars.enemeies is null");
            return;
        }

        if(GlobalVars.enemies == null) {
            Debug.Log("ERROR - GlobalVars.enemeies is null");
            return;
        }


        //
        foreach(KeyValuePair<Vector3Int , Stats> info in GlobalVars.enemies) {
            enemyCoords.Add(info.Key);
        }

        //
        currentEnemyIndex = 0;

        for(int i = 0; i < enemyCoords.Count; i++) {
            Vector3Int coord = enemyCoords[i];
            //Debug.Log("EnemeyCorrds Length: " + enemyCoords.Count);
            Stats stats = GlobalVars.enemies[coord];

            //
            Command command = null;

            if(stats.charType == "L1") {
                command = LevelOneAi(coord);
                // Debug.Log("Level 1 Ai");
            }
            else if(stats.charType == "L2" || stats.charType == "L3") {
                command = LevelTwoAi(coord);
                // Debug.Log("Level 2 Ai");
            }
            else {
                command = GeneralAi(coord);
                // Debug.Log("General Ai");
            }

            //
            if(command == null) {
                Debug.Log("ERROR - command for Enemy " + (currentEnemyIndex + 1) + " is null");
                continue;
            }

            //
            enemyCoords[currentEnemyIndex] = command.moveSpace;
            tm.commandQueue.Enqueue(command);

            //
            currentEnemyIndex++;
        }


        watch.Stop();
        Debug.Log("------------ TOTAL TIME FOR ENEMY TURN ------------");
        Debug.Log("Total Enemy AI Calcs: " + watch.ElapsedMilliseconds + " ms");
        Debug.Log("Total Scoring: " + totalTime + " ms");
        Debug.Log("Average Scoring: " + (totalTime/iterations) + " ms");

        tm.startPlayerTurn();
    }

    public List<KeyValuePair<Vector3Int , float>> getTiles(Stats enemyStats, Vector3Int currentEnemy) {

        List<KeyValuePair<Vector3Int, float>> tilesAndScores = new List<KeyValuePair<Vector3Int, float>>(); //Hex Coord, score

        int loopIterations = MAX_ITERATIONS;
        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , enemyStats.move, true)) {
            if(loopIterations <= 0) {
                Debug.Log("ERROR - Too many iterations");
                break;
            }

            if(GlobalVars.players.ContainsKey(info.Item1)) {
                continue;
            }

            if(enemyCoords.Contains(info.Item1) && info.Item1 != currentEnemy) {
                continue;
            }


            tilesAndScores.Add(new KeyValuePair<Vector3Int, float>(info.Item1, 0));
            loopIterations--;
        }

        if(tilesAndScores.Count == 0) {
            tilesAndScores.Add(new KeyValuePair<Vector3Int , float>(currentEnemy , 0));
        }

        //Debug.Log("Finished");

        return tilesAndScores;
    }

    /*********************************
        Types of Ai
    *********************************/

    public Command GeneralAi(Vector3Int coord) {
        Stats stats = GlobalVars.enemies[coord];

        //Check if play is really far from player
        Vector3Int tileToClosestPLayer = Vector3Int.one;//playerIsfar(stats , coord);


        Command command = null;

        //If ClosestPlayer is (1, 1, 1) (which is an invalid tile) then score tiles, other wise moce to closestPLayer
        if(tileToClosestPLayer == Vector3Int.one) {
            //Debug.Log("CLOSE MOVE");
            //Get Scores
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            List<KeyValuePair<Vector3Int , float>> tilesAndScore = GeneralScoreTiles(stats , coord);

            watch.Stop();
            totalTime += watch.ElapsedMilliseconds;
            iterations++;


            //Move Ai
            command = Move(tilesAndScore);

            //WriteToFile(tilesAndScore, command.moveSpace);

            if(command.moveSpace == command.startSpace) {
                command.moveSpace = Vector3Int.one;
            }
        }
        else {
            //Debug.Log("FAR MOVE");
            command = new Command(coord , tileToClosestPLayer);
        }

        if(command == null) {
            Debug.Log("ERROR - command is null");
            return null;
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

        Debug.Log("ENEMY AI (" + stats.charName + ")  " + command.attackTile);

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
        Complete Scoring Functions
    *********************************/

    Vector3Int playerIsfar(Stats enemyStats, Vector3Int currentEnemy) {
        //Variables that are reused
        Vector3Int cloestPlayer = Vector3Int.one;
        int closestPerson = -1;

        foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
            List<Vector3Int> temp = Pathfinding.PathBetweenPoints(playerInfo.Key , currentEnemy , true);

            int distance = closestPerson * 2;
            if(temp != null) {
                distance = temp.Count - 1;
            }

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
        foreach(Tuple<Vector3Int, int> info in Pathfinding.AllPossibleTiles(enemyCoords[currentEnemyIndex] , enemyStats.move, true)) {
            int distance = Pathfinding.PathBetweenPoints(cloestPlayer , info.Item1, true).Count - 1;

            if(distance < closestDistance) {
                closestDistance = distance;
                closestTile = info.Item1;
            }
        }

        return closestTile;
    }

    List<KeyValuePair<Vector3Int , float>> GeneralScoreTiles(Stats enemyStats, Vector3Int currentEnemy) {
        List<KeyValuePair<Vector3Int, float>> tilesAndScores = getTiles(enemyStats, currentEnemy);
        Debug.Log(tilesAndScores.Count);

        //Variables that are reused
        float score;
        List<int> distances;
        int closestPerson;
        int playersCanHit;

        playersCanHitList = new Dictionary<Vector3Int , int>();
       
        for(int tileScoreIndex = 0; tileScoreIndex < tilesAndScores.Count; tileScoreIndex++) {
            if(tileScoreIndex >= MAX_ITERATIONS) {
                Debug.Log("Error - Too many iterations");
                break;
            }

            //Find PLayer Distances Info
            distances = new List<int>();
            closestPerson = -1;
            playersCanHit = 0;

            //Get All distances
            int iterationsRemaining = MAX_ITERATIONS;

            foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
                iterationsRemaining--;

                if(iterationsRemaining <= 0) {
                    Debug.Log("Error - Too many iterations");
                }

                int distance = -1; // Default distance value
                if(tilesAndScores[tileScoreIndex].Key != null) {
                    List<Vector3Int> path = Pathfinding.PathBetweenPoints(playerInfo.Key , tilesAndScores[tileScoreIndex].Key, true);
                    if(path != null) {
                        distance = path.Count - 1;
                    }
                }
                distances.Add(distance);
            }

            //Get closestPerson distance
            iterationsRemaining = MAX_ITERATIONS;

            foreach(int dist in distances) {
                iterationsRemaining--;

                if(iterationsRemaining <= 0) {
                    Debug.Log("Error - Too many iterations");
                }

                if(closestPerson < dist) {
                    closestPerson = dist;
                }
            }

            //Get How Many Players can hit the current tile
            int distIndex = 0;
            foreach(KeyValuePair<Vector3Int, Stats> playerInfo in GlobalVars.players){
                if (distances[distIndex] <= playerInfo.Value.attackRange) {
                    playersCanHit++;
                }

                distIndex++;
            }

            playersCanHitList.Add(tilesAndScores[tileScoreIndex].Key, playersCanHit);


            //Scoring
            score = 0;
            score += Score_PlayersInEnemyRange(inRangeWeight , distances);
            score += Score_EnemyInPlayerRange(playerHitPenaltyWeight , playersCanHit);
            score += Score_FarAwayPenalty(farPlayerPenalty, closestPerson, enemyStats.move * 2);

            //Update Scoring
            KeyValuePair<Vector3Int, float> temp = tilesAndScores[tileScoreIndex];
            tilesAndScores[tileScoreIndex] = new KeyValuePair<Vector3Int , float>(temp.Key, score);
        }

        return tilesAndScores;
    }

    List<KeyValuePair<Vector3Int , float>> L1_ScoreTiles(Stats enemyStats , Vector3Int currentEnemy) {
        List<KeyValuePair<Vector3Int , float>> tilesAndScores = getTiles(enemyStats , currentEnemy);

        //Variables that are reused
        float score;
        int playersCanHit;

        playersCanHitList = new Dictionary<Vector3Int , int>();

        for(int tileScoreIndex = 0; tileScoreIndex < tilesAndScores.Count; tileScoreIndex++) {
            if(tileScoreIndex >= MAX_ITERATIONS) {
                Debug.Log("Error - Too many iterations");
                break;
            }

            // Set tile Score
            score = 0;

            //House Scoring
            score += Score_House(tilesAndScores[tileScoreIndex].Key, enemyStats);

            //Player Scoreing
            playersCanHit = 0;

            int iterationsRemaining = MAX_ITERATIONS;
            foreach(KeyValuePair<Vector3Int , Stats> playerInfo in GlobalVars.players) {
                iterationsRemaining--;

                if(iterationsRemaining <= 0) {
                    Debug.Log("Error - Too many iterations");
                }

                List<Vector3Int> tempPath = Pathfinding.PathBetweenPoints(playerInfo.Key , tilesAndScores[tileScoreIndex].Key , true);

                int distance = playerInfo.Value.attackRange * 2;
                if(tempPath != null) {
                    distance = tempPath.Count - 1;
                }

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
        Scoring Functions
    *********************************/
    private float Score_PlayersInEnemyRange(float weight, List<int> playerDistances) {
        float score = 0;

        foreach(int distance in playerDistances) {
            if(distance <= GlobalVars.enemies[enemyCoords[currentEnemyIndex]].attackRange)
                score += (GlobalVars.enemies[enemyCoords[currentEnemyIndex]].attackRange / (float)distance) * weight;
        }
        Debug.Log("PlayersInEnemyRange Score: " + score);

        return score;
    }

    private float Score_EnemyInPlayerRange(float weight, int playersCanHit) {
        Debug.Log("EnemyInPlayerRange Score: " + (-(playersCanHit * (playersCanHit + 1) * weight)));
        return -(playersCanHit * (playersCanHit + 1) * weight);
    }

    private float Score_FarAwayPenalty(float weight , int closestPerson , int maxDistance) {
        if(closestPerson >= maxDistance && closestPerson != -1) {
            // Calculate the penalty based on the distance to the closest player
            float penalty = weight * (1 - (float)closestPerson / maxDistance);

            Debug.Log("FarAwayPenalty: " + -penalty);
            return -penalty;
        }

        Debug.Log("FarAwayPenalty: " + 0);
        return 0;
    }

    private float Score_House(Vector3Int endTile, Stats enemyStats) {
        float score = 0;

        //House Scoring
        int closestHouse = -1;
        foreach(Vector3Int houseInfo in GlobalVars.L1_houseTiles) {
            List<Vector3Int> temp = Pathfinding.PathBetweenPoints(houseInfo , endTile , true);

            int distance = closestHouse + 1;
            if (temp != null)
            {
                distance = temp.Count - 1;
            }


            if(closestHouse > distance || closestHouse == -1) {
                closestHouse = distance;
            }
        }

        //Debug.Log(closestHouse + "\t" + tilesAndScores[tileScoreIndex].Key);
        if(closestHouse > enemyStats.attackRange)
            score += closestHouse * -20;

        return score;
    }


    /*********************************
        Actions
    *********************************/
    private Command Move(List<KeyValuePair<Vector3Int , float>> tilesAndScore) {
        // Ensure tilesAndScore is not empty
        if(tilesAndScore == null || tilesAndScore.Count == 0) {
            Debug.LogError("Error: tilesAndScore is empty or null.");
            return null; // or handle the error appropriately
        }

        // Get Max Value
        float maxValue = float.MinValue;

        foreach(KeyValuePair<Vector3Int , float> pair in tilesAndScore) {
            if(pair.Value > maxValue) {
                maxValue = pair.Value;
            }
        }

        // Get List of max value tiles
        List<Vector3Int> tiles = new List<Vector3Int>();

        foreach(KeyValuePair<Vector3Int , float> pair in tilesAndScore) {
            if(pair.Value == maxValue) {
                tiles.Add(pair.Key);
            }
        }

        //Debug.Log("Tile ocunt: " + tiles.Count);

        // Check for too many iterations
        if(tiles.Count == 0) {
            Debug.LogError("Error: No tiles with maximum score found.");
            return null; // or handle the error appropriately
        }

        // Pick one of the moves
        int randomMove = UnityEngine.Random.Range(0 , tiles.Count);
        //Debug.Log("Random Move: " + randomMove);
        Vector3Int moveTile = tiles[randomMove];

        // Debug
        debugMoveTiles = tiles;

        // Update enemy coordinates
        Command command = new Command(enemyCoords[currentEnemyIndex] , moveTile);
        enemyCoords[currentEnemyIndex] = moveTile;

        //Debug.Log("Finsih Move");

        return command;
    }


    private Command HouseAttack(Command command , Stats enemy) {
        command.attackTile = Vector3Int.one;

        foreach(Vector3Int houseTile in GlobalVars.L1_houseTiles) {
            if(attackedHouses.Contains(houseTile)) {
                continue;
            }

            //
            Vector3Int endTurnTile = command.moveSpace;
            if(endTurnTile != null) {
                endTurnTile = command.startSpace;
            }

            List<Vector3Int> temp = Pathfinding.PathBetweenPoints(houseTile , endTurnTile , true);
            if(temp == null)
                continue;

            int distance = temp.Count - 1;

            if(distance == -1)
                continue;

            if(distance <= enemy.attackRange) {
                command.houseAttackTile = houseTile;
                attackedHouses.Add(houseTile);
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

            //Debug.Log("PLayer: " + playerInfo.Key);
            //Debug.Log("Enemy: " + endTurnTile);

            List<Vector3Int> list = Pathfinding.PathBetweenPoints(playerInfo.Key , endTurnTile, false);
            if(list == null) {
                continue;
            }

            int distance = list.Count - 1;

            if(distance == -1 || distance == 0|| distance > enemy.attackRange)
                continue;

            //Debug.Log("DIST: " + distance);
            //Debug.Log("Distance: " + distance + "\nAttack Range: " + enemy.attackRange);
            //Debug.Log("Player Health: " + playerInfo.Value.curHealth + "\nLowest Health: " + lowestHealth);

            if(distance <= enemy.attackRange && playerInfo.Value.curHealth < lowestHealth) {
                //Debug.Log("Distance: " +  distance);
                lowestHealth = playerInfo.Value.curHealth;
                playerCoord = playerInfo.Key;
            }
        }

        //Debug.Log("END ATTACK");
        //Debug.Log("Attack Player at: " + playerCoord + "\nLowest Health: " + lowestHealth);

        command.attackTile = playerCoord;
       // Debug.Log("ATTACK COORD: " + playerCoord);


        return command;
    }


        /*********************************
            Debugging
        *********************************/

        void WriteToFile(List<KeyValuePair<Vector3Int , float>> tilesAndScores, Vector3Int enemyCoord) {
        // Check if the file already exists, if not, create it
        if(!File.Exists(filePath)) {
            File.Create(filePath).Close();
        }

        // Open the file to write
        using(StreamWriter writer = new StreamWriter(filePath)) {
            // Write class information to the file
            writer.WriteLine("Current Enemy: " + enemyCoord);
            writer.WriteLine("\nTile Scores: ");

            foreach(KeyValuePair<Vector3Int, float> tile in tilesAndScores) {
                writer.WriteLine(tile.Key + " - Score: " + tile.Value);
            }
            

            writer.Close();
            Debug.Log("Data written to file successfully!");
        }
    }
}