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

