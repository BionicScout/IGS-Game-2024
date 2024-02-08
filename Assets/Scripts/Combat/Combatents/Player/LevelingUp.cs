using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelingUp : MonoBehaviour
{
    private int move = 4;
    private int maxHealth = 10;
    private float dodgeChance;
    private int defense;
    private int power; 


    public void TurnipLvl()
    {
        foreach (KeyValuePair<Vector3Int, Stats> info in GlobalVars.players)
        {
            //gets each var in stats increases by one then updates the dictionary
            Debug.Log("Move: " + GlobalVars.players[new Vector3Int(0, 0, 0)].move);
            GlobalVars.players[new Vector3Int(0, 0 , 0)].move += 1;
            Debug.Log("New Move: " + GlobalVars.players[new Vector3Int(0, 0, 0)].move);

            Debug.Log("Power: " + GlobalVars.players[new Vector3Int(0, 0, 0)].power);
            GlobalVars.players[new Vector3Int(0, 0, 0)].power += 1;
            Debug.Log("New Power " + GlobalVars.players[new Vector3Int(0, 0, 0)].power);

            Debug.Log("Defense: " + GlobalVars.players[new Vector3Int(0, 0, 0)].defense);
            GlobalVars.players[new Vector3Int(0, 0, 0)].defense += 1;
            Debug.Log("New Defense: " + GlobalVars.players[new Vector3Int(0, 0, 0)].defense);

            Debug.Log("Max Health: " + GlobalVars.players[new Vector3Int(0, 0, 0)].maxHealth);
            GlobalVars.players[new Vector3Int(0, 0, 0)].maxHealth += 1;
            Debug.Log("New Max Health: " + GlobalVars.players[new Vector3Int(0, 0, 0)].maxHealth);


            //how lists and dictionarys kinda aer called
            //List<Stats> list = new List<Stats>();
            //list.Add(new Stats());
            //list[0];

            //Dictionary<Vector3Int, Stats> dict = new Dictionary<Vector3Int, Stats>();
            //dict.Add(new Vector3Int(1, -1, 0), new Stats());
            //dict[new Vector3Int(1, -1, 0)];



        }
    }

    public void ParsnipLvl()
    {
        foreach (KeyValuePair<Vector3Int, Stats> info in GlobalVars.players)
        {
            Stats move;
        }
    }

    public void BeetLvl()
    {
        foreach (KeyValuePair<Vector3Int, Stats> info in GlobalVars.players)
        {
            Stats move;
        }
    }
}

