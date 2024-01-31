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
        GlobalVars.enemies.Add(GlobalVars.centerHex , enemies[0]);
    }
}
