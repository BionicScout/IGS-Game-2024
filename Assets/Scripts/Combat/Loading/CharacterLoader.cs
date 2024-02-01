using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour {
    [SerializeField]
    public List<Stats> players;

    [SerializeField]
    public List<Stats> enemies;

    private void Start() {
        GlobalVars.players.Add(GlobalVars.centerHex, players[0]);
        GlobalVars.enemies.Add(GlobalVars.centerHex + 2 * Hex.hex_directions[0] , enemies[0]);

        GameObject currentTileObj = GlobalVars.hexagonTile[GlobalVars.centerHex];
        GameObject newTileObj = GlobalVars.hexagonTile[GlobalVars.centerHex + 2 * Hex.hex_directions[0]];

        //Update Sprite
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = players[0].sprite;
        newTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = enemies[0].sprite;


    }
}
