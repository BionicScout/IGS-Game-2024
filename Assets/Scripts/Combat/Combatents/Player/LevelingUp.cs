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
    public TextMeshProUGUI level2Txt;
    public TextMeshProUGUI level3Txt;

    public SpriteRenderer spriteIcon;
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
        //changes all information based on the selection number
        selectedOption = (selectedOption + 1) % GlobalVars.choosenPlayers.Count;
        if (selectedOption >= GlobalVars.choosenPlayers.Count)
        {
            selectedOption = 0;
        }
        MenuChanger();
        UpdateCharacter();
    }

    public void BackOption()
    {
        //changes all information based on the selection number
        selectedOption = (selectedOption - 1) % GlobalVars.choosenPlayers.Count;
        if (selectedOption < 0)
        {
            selectedOption = GlobalVars.choosenPlayers.Count - 1;
        }
        //checks what character is being loaded based of their name
        MenuChanger();
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
                GlobalVars.choosenPlayers[selectedOption].power += 3;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }

        //Changes for Range Characters
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Crossbowman")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 2;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].attackRange += 2;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }
        else if (GlobalVars.choosenPlayers[selectedOption].charType == "Archer")
        {
            if (GlobalVars.choosenPlayers[selectedOption].charLevel == 1)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 1;
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
            else if (GlobalVars.choosenPlayers[selectedOption].charLevel == 2)
            {
                GlobalVars.choosenPlayers[selectedOption].power += 2;
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
                GlobalVars.choosenPlayers[selectedOption].power += 2;
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
                GlobalVars.choosenPlayers[selectedOption].charLevel += 1;
            }
        }

        SceneSwapper.LoadHoldScene();
    }

    /*********************************
              Update Menus
    *********************************/
    public void MenuChanger()
    {
        //Melee Characters
        if (GlobalVars.choosenPlayers[selectedOption].charType == "Paladin")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Increase health to 25";
            level3Txt.text = "Increased Defense to 4";
        }
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Spearman")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Attack Range increases to 2";
            level3Txt.text = "Increase Power to 8";
        }
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Swordsman")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Power increases to 6";
            level3Txt.text = "Power increases to 7";
        }
        //range Characters
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Archer")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Increase power to 4";
            level3Txt.text = "Increase power to 6";
        }
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Crossbowman")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Increase Power Range to 5";
            level3Txt.text = "Increase Power Range to 7";
        }
        //Magic Characters
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Alchemist")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Power increases to 4";
            level3Txt.text = "Poison effects a 1 ring radius instead of 1 tile";
        }
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Cleric")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Healing effects a 1 ring radius instead of 1 tile";
            level3Txt.text = "Heal players for 6 HP";
        }
        else if(GlobalVars.choosenPlayers[selectedOption].charType == "Illusionist")
        {
            healthTxt.text = GlobalVars.choosenPlayers[selectedOption].maxHealth.ToString();
            powerTxt.text = GlobalVars.choosenPlayers[selectedOption].power.ToString();
            moveTxt.text = GlobalVars.choosenPlayers[selectedOption].move.ToString();
            defenseTxt.text = GlobalVars.choosenPlayers[selectedOption].defense.ToString();
            rangeTxt.text = GlobalVars.choosenPlayers[selectedOption].attackRange.ToString();
            level2Txt.text = "Smoke power is increased to 75";
            level3Txt.text = "Smoke effects a 2 ring radius";
        }
    }

}

