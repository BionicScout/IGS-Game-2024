using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour {
    [SerializeField]
    public List<Stats> players;

    [SerializeField]
    public List<Stats> enemies;

    private void Start() {
        //Player
        GlobalVars.players.Add(GlobalVars.centerHex, players[0]);
        GameObject currentTileObj = GlobalVars.hexagonTile[GlobalVars.centerHex];
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = players[0].sprite;

        //Random Offsets
        List<Vector3Int> offsets = new List<Vector3Int>();
        Vector3Int maxTile = GlobalVars.centerHex * 2;

        for(int i = 0; i < 15; i++) {
            Hex randHex = new Hex(Random.Range(0, maxTile.x), Random.Range(0 , maxTile.y));

            if(GlobalVars.hexagonTileRefrence[new Vector3Int(randHex.q, randHex.r, randHex.s)].isObstacle == false) {
                offsets.Add(new Vector3Int(randHex.q , randHex.r , randHex.s));
            }
        }

        //Enemey
        foreach(Vector3Int coord in offsets) {
            GlobalVars.enemies.Add(coord , enemies[0]);
            GameObject enemyTileObj = GlobalVars.hexagonTile[coord];
            enemyTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = enemies[0].sprite;
        }


    }
}
