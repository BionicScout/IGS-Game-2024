using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUtilites : MonoBehaviour {
    //TurnManager turnManager;

    private void Start() {
        //turnManager = FindAnyObjectByType<TurnManager>();


    }

    public bool InSmoke() { //REPEATEWD IN PlayerAction
        foreach(KeyValuePair<Vector3Int , Stats> coord in GlobalVars.players) {
            if(!GlobalVars.smokeTiles.ContainsKey(coord.Key)) {
                continue;
            }
            if(GlobalVars.smokeTiles[coord.Key] != 0) {
                GlobalVars.smokeTiles[coord.Key]--;
                return true;
            }
            if(GlobalVars.smokeTiles[coord.Key] == 0) {
                GlobalVars.smokeTiles.Remove(coord.Key);
            }
        }
        return false;
    }
}
