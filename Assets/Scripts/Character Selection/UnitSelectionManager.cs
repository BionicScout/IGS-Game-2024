using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class UnitSelectionManager : MonoBehaviour {
    public SingleCharacterSelection[] selections = new SingleCharacterSelection[4];
    public List<Stats> playerStats = new List<Stats>(4); 

    public void Start () { 
        for(int i = 0; i < 4; i++) {
            selections[i].Load(this, i);
        }

        SceneSwapper.setCurrentScene();
    }

    public void Submit() {
        GlobalVars.choosenPlayers.Clear();

        foreach(SingleCharacterSelection s in selections) {
            GlobalVars.choosenPlayers.Add(playerStats[s.selectedOption].Copy());
        }

        //HAVE TO CHANGE THIS BACK, ONLY FOR TESTING OURPUSES
        SceneSwapper.A_LoadScene("LevelingUp");
    }
}
