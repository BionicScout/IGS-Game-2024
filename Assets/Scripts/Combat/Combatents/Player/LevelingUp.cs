using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelingUp : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI powerTxt;
    public TextMeshProUGUI moveTxt;
    public TextMeshProUGUI defenseTxt;
    public TextMeshProUGUI rangeTxt;
    public TextMeshProUGUI nextLvlTxt;

    SpriteRenderer spriteIcon;
    public Button nextBTN, backBTN;

    public int selectedOption = 0;


    private void Start()
    {
        spriteIcon = GetComponent<SpriteRenderer>();
        NextOption();
        BackOption();
        GlobalVars.levelClear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneSwapper.LoadHoldScene();
        }
    }

    public void NextOption()
    {
        //checks what character is being loaded based of their name
        SwordsmanLvl();
        PaladinLvl();
        SpearmanLvl();
        CrossbowLvl();
        ArcherLvl();
        ClericLvl();
        AlchemistLvl();
        IllusionistLvl();
        //changes all information based on the selection number
        selectedOption = (selectedOption + 1) % GlobalVars.choosenPlayers.Count;
        if (selectedOption >= GlobalVars.choosenPlayers.Count)
        {
            selectedOption = 0;
        }
        UpdateCharacter();
    }

    public void BackOption()
    {
        //checks what character is being loaded based of their name
        SwordsmanLvl();
        PaladinLvl();
        SpearmanLvl();
        CrossbowLvl();
        ArcherLvl();
        ClericLvl();
        AlchemistLvl();
        IllusionistLvl();
        //changes all information based on the selection number
        selectedOption = (selectedOption - 1) % GlobalVars.choosenPlayers.Count;
        if (selectedOption < 0)
        {
            selectedOption = GlobalVars.choosenPlayers.Count - 1;
        }
        UpdateCharacter();
    }
    //changes the name and sprite depending on selection number
    private void UpdateCharacter()
    {
        Stats stats = GlobalVars.choosenPlayers[selectedOption];
        spriteIcon.sprite = stats.sprite;
        nameTxt.text = stats.charType;
    }

    //Actually changes the stats
    public void LevelUpCharacter()
    {
        //Changes for Melee Characters
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Swordsman")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1) 
            { 
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Paladin")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].maxHealth += 5;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].defense += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Spearman")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        
        //Changes for Range Characters
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Crossbowman")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 4;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if(GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 2;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Archer")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        
        //Changes for Magic Characters
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Cleric")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 2;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Alchemist")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Illusionist")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 15;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }

        SceneSwapper.LoadHoldScene();
    }

    /*********************************
              Update Menus
    *********************************/

    //Updates for all Melee charcters
    public void SwordsmanLvl()
    {
        //can check their character name
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Swordsman")
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
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
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

    }
    public void PaladinLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Paladin")
        {
            Debug.Log("Checked if was paladin");
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                Debug.Log("Checked if Lvl 1");
                //updeates the preview menu
                Debug.Log(GlobalVars.choosenPlayers[selectedOption].maxHealth);
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString() + " + 5";
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Next Level: Increased Defense";
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString() + " + 1";
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Fully Upgraded";
            }
        }
    }
    public void SpearmanLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Spearman")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString() + " + 1";
                nextLvlTxt.text = "Next Level: Attack in range for half damage";
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Fully Upgraded";
            }
        }
    }
        
    //Updates all Range Characters
    public void CrossbowLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Crossbowman")
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
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
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
    public void ArcherLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Archer")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: 2 attacks for 1 action";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString() + " + 4";
                nextLvlTxt.text = "Next Level: 3 attacks for 1 action";
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: 3 attacks for 1 action";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Next Level: Fully Upgraded";
            }
        }
    }

    //Updates all Magic characters
    public void ClericLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Cleric")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: Heal all in a 1 tile radius";
                nextLvlTxt.text = "Next Level: Increased Healing Power";
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 1";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Fully Upgraded";
            }
        }
    }
    public void AlchemistLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Alchemist")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 2";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Next Level: Poision in 1 tile raduis";
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: Poision all in 1 tile radius";
                nextLvlTxt.text = "Fully Upgraded";
            }
        }
    }
    public void IllusionistLvl()
    {
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Illusionist")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                //updeates the preview menu
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString() + " + 15";
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
                nextLvlTxt.text = "Next Level: Increased Range";
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                healthTxt.text = "Max Health: " + GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
                powerTxt.text = "Power: " + GlobalVars.choosenPlayers[selectedOption].power.ToString();
                moveTxt.text = "Movement: " + GlobalVars.choosenPlayers[selectedOption].move.ToString();
                defenseTxt.text = "Defense: " + GlobalVars.choosenPlayers[selectedOption].defense.ToString();
                rangeTxt.text = "Range: " + GlobalVars.choosenPlayers[selectedOption].attackRange.ToString() + " + 1";
                nextLvlTxt.text = "Fully Upgraded";
            }
        }
    }
}

