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

    public int selectedOption = 1;


    private void Start()
    {
        spriteIcon = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        TurnipLvl();
        ParsnipLvl();
        //BeetLvl();
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

    //for the button to use
    public void LevelUpCharacter()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Turnip")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == GlobalVars.choosenPlayers[selectedOption].charLevel + 1) 
            { 
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;

                Debug.Log(GlobalVars.choosenPlayers[selectedOption].power);
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].charLevel);
            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == GlobalVars.choosenPlayers[selectedOption].charLevel + 1)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;

                Debug.Log(GlobalVars.choosenPlayers[selectedOption].power);
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].charLevel);
            }
        }
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Parsnip")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 4;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;

                Debug.Log(GlobalVars.choosenPlayers[selectedOption].power);
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].charLevel);
            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 2;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;

                Debug.Log(GlobalVars.choosenPlayers[selectedOption].power);
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].charLevel);
            }
        }
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Beet")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;

                Debug.Log(GlobalVars.choosenPlayers[selectedOption].power);
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].charLevel);
            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;

                Debug.Log(GlobalVars.choosenPlayers[selectedOption].power);
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].charLevel);
            }
        }
    }

    //will update all menus
    public void TurnipLvl()
    {
        //can check their character name
        if (GlobalVars.choosenPlayers[selectedOption].charName == "Turnip")
        {
            if(GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 1";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Next Level: Increased power";
            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 1";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Fully Upgraded";
            }
        }

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
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString() + " + 4";
                nextLvlTxt.text = "Next Level: Increased Power";
            }
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + "2";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Next Level: Fully Upgraded";
            }
        }
    }

    //public void BeetLvl()
    //{
    //    if (GlobalVars.choosenPlayers[selectedOption].charName == "Beet")
    //    {
    //        if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
    //        {
    //            //updeates the preview menu
    //            healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
    //            powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
    //            moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
    //            defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
    //            rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString() + " + 1";
    //            nextLvlTxt.text = "Next Level: Increased Healing Power";
    //        }
    //        if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
    //        {
    //            healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
    //            powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 1";
    //            moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
    //            defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
    //            rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
    //            nextLvlTxt.text = "Fully Upgraded";
    //        }
    //    }
    //}
}

