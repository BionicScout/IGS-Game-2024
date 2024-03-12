using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingUp : MonoBehaviour
{
    public TextMeshProUGUI nameTxt, healthTxt, powerTxt, moveTxt, defenseTxt, rangeTxt, nextLvlTxt;
    SpriteRenderer spriteIcon;
    public Button nextBTN, backBTN;

    public int selectedOption = 0;


    private void Start()
    {
        spriteIcon = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        TurnipLvl();
        ParsnipLvl();
        BeetLvl();
    }

    public void NextOption()
    {
        selectedOption = (selectedOption + 1) % GlobalVars.choosenPlayers.Count;
        if (selectedOption >= GlobalVars.choosenPlayers.Count)
        {
            selectedOption = 0;
        }
        UpdateCharacter();
    }

    public void BackOption()
    {
        selectedOption = (selectedOption - 1) % GlobalVars.choosenPlayers.Count;
        if (selectedOption < 0)
        {
            selectedOption = GlobalVars.choosenPlayers.Count - 1;
        }
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        Stats stats = GlobalVars.choosenPlayers[selectedOption];
        spriteIcon.sprite = stats.sprite;
        nameTxt.text = stats.charName;
    }

    public void TurnipLvl()
    {
        //can check their character name
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Turnip")
        {
            if(GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString() + " + 1";
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 1";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString() + " + 1";
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString() + " + 1";
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString() + " + 1";
                nextLvlTxt.text = "Next Level: Extra Damge";
            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {

            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 3)
            {

            }
        }
            //gets each var in stats increases by one then updates the dictionary
            Debug.Log("Move: " + GlobalVars.players[new Vector3Int(0, 0, 0)].move);
            GlobalVars.players[new Vector3Int(0, 0, 0)].move += 1;
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

    public void ParsnipLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Parsnip")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {

            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {

            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 3)
            {

            }
        }
        //gets each var in stats increases by one then updates the dictionary
        Debug.Log("Move: " + GlobalVars.players[new Vector3Int(0, 0, 0)].move);
        GlobalVars.players[new Vector3Int(0, 0, 0)].move += 1;
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
    }

    public void BeetLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Beet")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {

            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {

            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 3)
            {

            }
        }
        //gets each var in stats increases by one then updates the dictionary
        Debug.Log("Move: " + GlobalVars.players[new Vector3Int(0, 0, 0)].move);
        GlobalVars.players[new Vector3Int(0, 0, 0)].move += 1;
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
    }
}

