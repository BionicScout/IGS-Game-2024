using UnityEngine;
using System.Collections.Generic;
using System;

public class SpawnInfoLoader : MonoBehaviour {
    public TextAsset spawnDataFile;

    [System.Serializable]
    public class SpawnData {
        public List<SpawnLocation> PlayerSpawns;
        public List<EnemySpawn> EnemySpawnLocations;
        public Dictionary<string , List<EnemySpawn>> Turns;
    }

    [System.Serializable]
    public class SpawnLocation {
        public Vector3Int spawnLocation;
    }

    [System.Serializable]
    public class EnemySpawn {
        public string unitName;
        public Vector3Int spawnLocation;
    }


    void Start() {
        if(spawnDataFile != null) {
            LoadSpawnData();
        }
        else {
            Debug.LogError("Spawn data file not assigned!");
        }
    }


    void LoadSpawnData() {
        //Get Data
        string json = spawnDataFile.text;
        SpawnData data = JsonUtility.FromJson<SpawnData>(json);

        //Load Player Spawn into SpawnWave
        SpawnWave playerWave = new SpawnWave();
        foreach(SpawnLocation player in data.PlayerSpawns) {
            playerWave.spawns.Add(new Tuple<string , Vector3Int>("Player" , player.spawnLocation));
        }

        //Load Enemy Spawn into SpawnWave
        SpawnWave enemyWave = new SpawnWave();
        foreach(EnemySpawn enemy in data.EnemySpawnLocations) {
            enemyWave.spawns.Add(new Tuple<string , Vector3Int>(enemy.unitName , enemy.spawnLocation));
        }

        //Load Enemy After Start Spawns into SpawnWave
        List<SpawnWave> laterWaves = new List<SpawnWave>();

        foreach(KeyValuePair<string, List<EnemySpawn>> turn in data.Turns) {
            SpawnWave wave = new SpawnWave();

            foreach(EnemySpawn enemy in turn.Value) {
                wave.spawns.Add(new Tuple<string , Vector3Int>(enemy.unitName , enemy.spawnLocation));
            }

            laterWaves.Add(wave);
        }

        Debug.Log(laterWaves);
    }


}
