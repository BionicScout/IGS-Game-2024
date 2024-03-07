using UnityEngine;
using System.Collections.Generic;
using System;

public class SpawnInfoLoader : MonoBehaviour {
    public TextAsset spawnDataFile;

    [System.Serializable]
    public class SpawnData {
        public List<SpawnLocation> PlayerSpawns;
        public List<EnemySpawn> EnemySpawnLocations;
        public List<TurnData> Turns;
    }

    [System.Serializable]
    public class TurnData {
        public int TurnNumber;
        public List<EnemySpawn> EnemySpawns;
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
            Queue<SpawnWave> queue = LoadSpawnData();

            CharacterLoader cl = FindAnyObjectByType<CharacterLoader>();
            cl.playerSpawns = queue.Dequeue();
            cl.enemySpawns = queue.Dequeue();

            GlobalVars.spawnWaves = queue;
        }
        else {
            Debug.LogError("Spawn data file not assigned!");
        }
    }


    Queue<SpawnWave> LoadSpawnData() {
        //Get Data
        string json = spawnDataFile.text;
        SpawnData data = JsonUtility.FromJson<SpawnData>(json);
        print(json);

        //
        Queue<SpawnWave> spawnQueue = new Queue<SpawnWave>();

        //Load Player Spawn into Queue
        SpawnWave playerWave = new SpawnWave();
        foreach(SpawnLocation player in data.PlayerSpawns) {
            playerWave.spawns.Add(new Tuple<string , Vector3Int>("Player" , player.spawnLocation));
        }
        spawnQueue.Enqueue(playerWave);

        //Load Enemy Spawn into Queue
        SpawnWave enemyWave = new SpawnWave();
        foreach(EnemySpawn enemy in data.EnemySpawnLocations) {
            enemyWave.spawns.Add(new Tuple<string , Vector3Int>(enemy.unitName , enemy.spawnLocation));
        }
        spawnQueue.Enqueue(enemyWave);

        //Load Enemy Waves After Start into Queue
        SpawnWave wave;
        int waveNumber = 1;

        foreach(TurnData turn in data.Turns) {

            while(waveNumber < turn.TurnNumber) {
                spawnQueue.Enqueue(new SpawnWave());
                waveNumber++;
            }

            wave = new SpawnWave();
            foreach(EnemySpawn enemy in turn.EnemySpawns) {
                wave.spawns.Add(new Tuple<string , Vector3Int>(enemy.unitName , enemy.spawnLocation));
            }

            waveNumber++;
            spawnQueue.Enqueue(wave);
        }

        //Debug Queue
        string str = "";
        for(int i = 0; i < spawnQueue.Count; i++) {
            SpawnWave wave2 = spawnQueue.Dequeue();
            str += wave2.ToString() + "\n\n";
            spawnQueue.Enqueue(wave2);
        }

        Debug.Log(str);


        return spawnQueue;
    }


}
