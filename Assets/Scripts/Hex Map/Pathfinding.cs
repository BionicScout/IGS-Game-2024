using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {
    private const int UNVISITED_DISTANCE = -1;
    private const int MAX_ITERATIONS = 2000;




    /*********************************
        Pathfinding Structs
    *********************************/
    public struct SearchHex {
        public Vector3Int coord;
        public bool visited;
        public bool isObstacle;
        public int dist;
        public Vector3Int previous;

        // Constructor
        public SearchHex(Vector3Int hexCoord) {
            coord = hexCoord;
            visited = false;
            TileScriptableObjects template = GlobalVars.hexagonTileRefrence[hexCoord];
            isObstacle = template.isObstacle;
            dist = UNVISITED_DISTANCE;
            previous = Vector3Int.one; //Impossible to Get with Hexagon Coords
        }
    }

    /*********************************
       Pathfinding
    *********************************/

    public static List<Tuple<Vector3Int , int>> AllPossibleTiles(Vector3Int startPos , int range) {
        /*Breadth First Search*/
        Dictionary<Vector3Int , SearchHex> searchList = createSearchList(startPos);

        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(startPos);

        searchList = visit(searchList , startPos);

        while(queue.Count > 0) {
            Vector3Int currentCoord = queue.Dequeue();

            foreach(Vector3Int dir in Hex.hex_directions) {
                Vector3Int nextTile = currentCoord + dir;

                //Avoid Off Baord
                if(!GlobalVars.availableHexes.Contains(nextTile)) {
                    continue;
                }

                //Avoid Obsticale
                if(searchList[nextTile].isObstacle) {
                    searchList = visit(searchList , nextTile);
                    continue;
                }

                //Already Visited
                if(searchList[nextTile].visited) {
                    continue;
                }

                //Visit
                queue.Enqueue(nextTile);
                searchList = visitAndRange(searchList , nextTile, searchList[currentCoord].dist);
            }
        }

        //return List of Possiblities
        List<Tuple<Vector3Int, int>> possibleTiles = new List<Tuple<Vector3Int, int>>();

        foreach(KeyValuePair<Vector3Int, SearchHex> data in searchList) {
            if(data.Value.dist <= range && !data.Value.isObstacle) {
                possibleTiles.Add(new Tuple<Vector3Int, int>(data.Value.coord, data.Value.dist));
            }
        }

        return possibleTiles;
    }

    public static List<Vector3Int> PathBetweenPoints(Vector3Int startPos , Vector3Int endPoint) {
        /*Breadth First Search*/
        Dictionary<Vector3Int , SearchHex> searchList = createSearchList(startPos);

        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(startPos);

        searchList = visit(searchList , startPos);

        //int max_iteration = MAX_ITERATIONS;

        while(queue.Count > 0 /*&& max_iteration > 0*/) {
            Vector3Int currentCoord = queue.Dequeue();

            foreach(Vector3Int dir in Hex.hex_directions) {
                Vector3Int nextTile = currentCoord + dir;

                //Avoid Off Baord
                if(!GlobalVars.availableHexes.Contains(nextTile)) {
                    continue;
                }

                //Avoid Obsticale
                if(searchList[nextTile].isObstacle) {
                    searchList = visit(searchList , nextTile);
                    continue;
                }

                //Already Visited
                if(searchList[nextTile].visited) {
                    continue;
                }

                //Visit
                queue.Enqueue(nextTile);
                searchList = visitAndPrevious(searchList , nextTile , currentCoord);
            }

            //max_iteration--;
        }

        //Debug.Log("Max Iterations: " + max_iteration);

        //if(max_iteration == 0)
        //    return new List<Vector3Int>();

        //return List of Possiblities
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentPath = endPoint;

        //Debug.Log("FIND PATH");
        //Debug.Log("Current Path: " + currentPath);
        while(searchList[currentPath].previous != Vector3.one) {
            //Debug.Log("Current Path: " + currentPath);
            path.Add(currentPath);
            currentPath = searchList[currentPath].previous;
        }
        //Debug.Log("FOUND PATH");

        path.Add(currentPath);
        path.Reverse();

        return path;
    }




    /*********************************
        Smaller methods
    *********************************/

    private static Dictionary<Vector3Int , SearchHex> createSearchList(Vector3Int startPos) {
        Dictionary<Vector3Int , SearchHex> searchList = new Dictionary<Vector3Int , SearchHex>();

        foreach(Vector3Int pos in GlobalVars.availableHexes) {
            SearchHex data = new SearchHex(pos);
            searchList.Add(pos , data);
        }

        //Start Pos = 0
        SearchHex temp = searchList[startPos];

        temp.dist = 0;

        searchList[startPos] = temp;


        return searchList;
    }

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

    private static Dictionary<Vector3Int , SearchHex> visitAndPrevious(Dictionary<Vector3Int , SearchHex> searchList , Vector3Int key , Vector3Int previousCoord) {
        SearchHex temp = searchList[key];

        temp.visited = true;
        //temp.dist = currentRange + 1;
        temp.previous = previousCoord;

        searchList[key] = temp;
        return searchList;
    }
}
