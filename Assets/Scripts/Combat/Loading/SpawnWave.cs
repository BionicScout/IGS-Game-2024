using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWave {
    public List<Tuple<string, Vector3Int>> spawns = new List<Tuple<string, Vector3Int>>();

    public override string ToString() {
        string str = "Spawn Wave:";

        foreach (var item in spawns) {
            str += "\n" + item.Item1 + " - " + item.Item2;
        }

        return str;
    }
}
