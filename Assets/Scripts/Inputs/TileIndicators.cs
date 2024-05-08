using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileIndicators : MonoBehaviour {
    TurnManager turnManager;

    //List<Vector3Int> tilesToClear = new List<Vector3Int>();
    //List<Vector3Int> tilesClearType = new List<Vector3Int>();

    private void Start() {
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    public void MoveIndicators(bool onOff, Vector3Int playerCoord) {
        int moveRange = turnManager.getMovementLeft(playerCoord);
        if(moveRange >= 0) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , moveRange - 1 , true)) {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
            }
        }
        else if(moveRange == 0) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1 , true)) {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
            }
        }
    }
    public void AttackIndicators(bool onOff , Vector3Int playerCoord , int attackRange) {
        if(!turnManager.getActionUse(playerCoord)) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , attackRange - 1 , false)) {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(onOff);
            }
        }
    }
    public void HealIndicators(bool onOff, Vector3Int playerCoord) {
        if(!turnManager.getActionUse(playerCoord)) {
            foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 1 , false)) {
                Vector3Int t = temp.Item1;
                GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(onOff);
            }
        }
    }
    public void InteractIndicators(Vector3Int playerCoord) {
        if(turnManager.getActionUse(playerCoord)) {
            foreach(KeyValuePair<Vector3Int , TileScriptableObjects> temp in GlobalVars.hexagonTileRefrence) {
                Vector3Int t = temp.Key;
                foreach(Tuple<Vector3Int , int> temp2 in Pathfinding.AllPossibleTiles(playerCoord , 1 , true)) {
                    if(temp.Value.interactable) {
                        Vector3Int t2 = temp2.Item1;
                        GlobalVars.hexagonTile[t2].transform.GetChild(5).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public void ClearIndicators(Vector3Int playerCoord) {
        foreach(Tuple<Vector3Int , int> temp in Pathfinding.AllPossibleTiles(playerCoord , 50 , false)) {
            Vector3Int t = temp.Item1;
            GlobalVars.hexagonTile[t].transform.GetChild(3).gameObject.SetActive(false);
            GlobalVars.hexagonTile[t].transform.GetChild(4).gameObject.SetActive(false);
        }
    }
}
