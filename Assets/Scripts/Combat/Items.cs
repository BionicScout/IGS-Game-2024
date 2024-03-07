using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public int singleHealAMT, powerBuffAMT, defenseBuffAMT, reviveAMT, healScrollAMT;

    // Start is called before the first frame update
    void Start()
    {
        //singleHealAMT = 0;
        //powerBuffAMT = 0;
        //defenseBuffAMT = 0;
        //reviveAMT = 0;
        //healScrollAMT = 0;

    }

    public void HealScroll()
    {
        for(int i = 0; i < GlobalVars.players.Count; i++)
        {
            //Vector3Int playerCoords = GlobalVars.players[i];
            //GlobalVars.players.health
        }
    }

    public void ClearItems()
    {
        singleHealAMT = 0;

        powerBuffAMT = 0;
        defenseBuffAMT = 0;
        reviveAMT = 0;
        healScrollAMT = 0;
    }
}
