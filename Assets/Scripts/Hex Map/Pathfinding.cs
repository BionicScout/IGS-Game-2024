using System;
using System.Collections.Generic;
using UnityEngine;


public class Pathfinding {
    private const int UNVISITED_DISTANCE = -1;
    private const int MAX_ITERATIONS = 2000;

    /*********************************
        Pathfinding Data Structures
    *********************************/
    public struct SearchHex {
        public Vector3Int coord;
        public bool visited;
        public bool isObstacle;
        public int dist;
        public Vector3Int previous;

        // Constructor
        public SearchHex(Vector3Int hexCoord, bool ignoreUnits) {
            if(!GlobalVars.hexagonTileRefrence.ContainsKey(hexCoord)) {
                Debug.LogError("ERROR - Invalid hexagon coordinate provided: " + hexCoord);
            }

            coord = hexCoord;
            visited = false;
            TileScriptableObjects template = GlobalVars.hexagonTileRefrence[hexCoord];
            isObstacle = template.isObstacle || (ignoreUnits && (GlobalVars.players.ContainsKey(hexCoord) || GlobalVars.enemies.ContainsKey(hexCoord)));
            dist = UNVISITED_DISTANCE;
            previous = Vector3Int.one; //Impossible to Get with Hexagon Coords
        }

        public SearchHex(Vector3Int hexCoord , bool ignoreUnits, Vector3Int startLoc) {
            if(!GlobalVars.hexagonTileRefrence.ContainsKey(hexCoord)) {
                Debug.LogError("ERROR - Invalid hexagon coordinate provided: " + hexCoord);
            }

            coord = hexCoord;
            visited = false;
            TileScriptableObjects template = GlobalVars.hexagonTileRefrence[hexCoord];
            isObstacle = template.isObstacle || (ignoreUnits && startLoc != hexCoord && (GlobalVars.players.ContainsKey(hexCoord) || GlobalVars.enemies.ContainsKey(hexCoord)));
            dist = UNVISITED_DISTANCE;
            previous = Vector3Int.one; //Impossible to Get with Hexagon Coords
        }
    }

    public class PriorityQueue<Vector3Int> {
        private List<Tuple<Vector3Int, float>> elements = new List<Tuple<Vector3Int, float>>();

        public int Count {
            get { return elements.Count; }
        }

        public void Enqueue(Vector3Int item , float priority) {
            elements.Add(Tuple.Create(item , priority));
        }

        public Vector3Int Dequeue() {
            int bestIndex = 0;
            for(int i = 0; i < elements.Count; i++) {
                if(elements[i].Item2 < elements[bestIndex].Item2) {
                    bestIndex = i;
                }
            }

            Vector3Int bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

        public bool Contains(Vector3Int item) {
            foreach(var element in elements) {
                if(EqualityComparer<Vector3Int>.Default.Equals(element.Item1 , item)) {
                    return true;
                }
            }
            return false;
        }
    }


/*********************************
   Pathfinding
*********************************/

public static List<Tuple<Vector3Int , int>> AllPossibleTiles(Vector3Int startPos , int range, bool ignoreUnits) {
        // Check for valid start position
        if(!GlobalVars.availableHexes.Contains(startPos)) {
            Debug.LogError("ERROR - Invalid start position provided");
            return null; 
        }

        // Check for valid range
        if(range < 0) {
            Debug.LogError("ERROR - Invalid range provided.");
            return null; 
        }

        // Check for missing references
        if(GlobalVars.availableHexes == null || Hex.hex_directions == null) {
            Debug.LogError("ERROR - Missing references.");
            return null;
        }

        // Define variables for loop
        Vector3Int currentCoord;
        Vector3Int nextTile;
        SearchHex nextSearchHex;

        // Create dictionary with start hex
        Dictionary<Vector3Int , SearchHex> searchList = new Dictionary<Vector3Int , SearchHex> {
            { startPos, new SearchHex(startPos, ignoreUnits) }
        };

        // Define queue with tiles to search. Starts with the starting location. Then Search list.
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(startPos);

        searchList = visit(searchList , startPos);

        int iterations = 0;

        while(queue.Count > 0) {
            currentCoord = queue.Dequeue();
            iterations++;

            foreach(Vector3Int dir in Hex.hex_directions) {
                nextTile = currentCoord + dir;

                // If position is off board, skip.
                if(!GlobalVars.availableHexes.Contains(nextTile)) {
                    continue;
                }

                // If position is on board, add Search hex if needed
                if(!searchList.ContainsKey(nextTile)) {
                    nextSearchHex = new SearchHex(nextTile , ignoreUnits);
                    searchList.Add(nextTile , nextSearchHex);
                }
                else {
                    nextSearchHex = searchList[nextTile];
                }

                // Avoid Obstacle
                if(nextSearchHex.isObstacle) {
                    searchList = visit(searchList , nextTile);
                    continue;
                }

                // Already Visited
                if(nextSearchHex.visited) {
                    continue;
                }

                // Visit
                queue.Enqueue(nextTile);
                searchList = visitAndRange(searchList , nextTile , searchList[currentCoord].dist);
            }

            // Check for potential infinite loop
            if(iterations >= MAX_ITERATIONS) {
                Debug.LogError("ERROR - Maximum iterations reached. Potential infinite loop detected.");
                return null;
            }
        }


        // Return List of Possibilities
        List<Tuple<Vector3Int , int>> possibleTiles = new List<Tuple<Vector3Int , int>>();

        foreach(KeyValuePair<Vector3Int , SearchHex> data in searchList) {
            if(data.Value.dist <= range && !data.Value.isObstacle) {
                possibleTiles.Add(new Tuple<Vector3Int , int>(data.Value.coord , (int)data.Value.dist)); //Only int for dist in this algorithm
            }
        }

        return possibleTiles;
    }

    public static List<Vector3Int> PathBetweenPoints(Vector3Int startPos , Vector3Int endPoint, bool ignoreUnits) {
        // A* (star) Pathfinding

        //Loop Variables 
        SearchHex nextSearchHex;

        // Check if start and end positions are valid
        if(!GlobalVars.availableHexes.Contains(startPos)) {
            Debug.LogError("ERROR - Invalid start position provided: " + startPos);
            return null;
        }
        if(!GlobalVars.availableHexes.Contains(endPoint)) {
            Debug.LogError("ERROR - Invalid end position provided: " + endPoint);
            return null;
        }

        //Initilzize frontier with Priority Queue
        PriorityQueue<Vector3Int> frontier = new PriorityQueue<Vector3Int>();
        frontier.Enqueue(startPos, 0);

        //Define dictionary of tile Activley in search
        Dictionary<Vector3Int, SearchHex> searchList = new Dictionary<Vector3Int , SearchHex>();
        SearchHex temp = new SearchHex(startPos, ignoreUnits, startPos);
        temp.dist = 0;
        searchList.Add(startPos, temp);

        //
        int iterations = 0;
        while(frontier.Count != 0){
            Vector3Int currentTilePos = frontier.Dequeue();

            //If target found exit
            if(currentTilePos == endPoint) {
                Debug.Log(searchList[endPoint].previous);
                break;
            }

            //
            foreach(Vector3Int dir in Hex.hex_directions) {
                Vector3Int nextTile = currentTilePos + dir;

                // If position is off board, skip.
                if(!GlobalVars.availableHexes.Contains(nextTile)) {
                    continue;
                }

                // If position is on board, add Search hex if needed
                if(!searchList.ContainsKey(nextTile)) {
                    nextSearchHex = new SearchHex(nextTile , ignoreUnits, startPos);
                    searchList.Add(nextTile , nextSearchHex);
                }
                else {
                    nextSearchHex = searchList[nextTile];
                }

                // Avoid Obstacle
                if(nextSearchHex.isObstacle) {
                    continue;
                }

                //
                SearchHex currentSearchHex = searchList[currentTilePos];
                int newDist = (currentSearchHex.dist + 1); 

                if(nextSearchHex.dist == UNVISITED_DISTANCE || nextSearchHex.dist > newDist) {
                    nextSearchHex.dist = newDist;
                    float priority = newDist + ManhattanDistance(currentTilePos , endPoint);
                    frontier.Enqueue(nextTile, priority);
                    nextSearchHex.previous = currentTilePos;

                    searchList[nextTile] = nextSearchHex;
                }
            }

            //
            iterations++;

            if(iterations >= MAX_ITERATIONS) {
                Debug.LogError("ERROR - Maximum iterations reached. Potential infinite loop detected.");
                return null;
            }
        }

        //Doesn't Reach Target
        if(!searchList.ContainsKey(endPoint)) {
            Debug.Log("Pathfinding - end point not reached");
            return null;
        }



        //Trace Path back
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentPath = endPoint;

        // Maximum iteration limit for path reconstruction
        int pathIterations = 0;
        //Debug.Log("---------------------------------------------------------------------------------------------------");
        //Debug.Log(currentPath + "\t" + searchList.ContainsKey(currentPath));

        while(searchList[currentPath].previous != Vector3.one) {
            // Increment path reconstruction iteration count
            pathIterations++;

            // Check maximum iteration limit
            if(pathIterations >= MAX_ITERATIONS) {
                Debug.LogError("ERROR - Maximum path reconstruction iterations reached. Potential infinite loop detected.");
                return null;
            }

            // Add current path to the list and Move to the previous tile
            path.Add(currentPath);
            currentPath = searchList[currentPath].previous;
            //Debug.Log(currentPath);
        }

        // Add the last path
        path.Add(currentPath);
        path.Reverse();

        return path;

    }

    // Heuristic function using Manhattan distance
    private static float ManhattanDistance(Vector3Int from , Vector3Int to) {
        int dx = Mathf.Abs(from.x - to.x);
        int dy = Mathf.Abs(from.y - to.y);
        int dz = Mathf.Abs(from.z - to.z);
        return dx + dy + dz;
    }

    /*********************************
        Smaller methods
    *********************************/

    private static Dictionary<Vector3Int , SearchHex> visit(Dictionary<Vector3Int , SearchHex> searchList , Vector3Int key) {
        SearchHex temp = searchList[key];

        temp.visited = true;

        searchList[key] = temp;
        return searchList;
    }

    private static Dictionary<Vector3Int , SearchHex> visitAndRange(Dictionary<Vector3Int , SearchHex> searchList , Vector3Int key , int currentRange) {
        SearchHex temp = searchList[key];

        temp.visited = true;
        temp.dist = currentRange + 1;

        searchList[key] = temp;
        return searchList;
    }
}
