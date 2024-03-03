using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    public CharacterData characterData;
    Characters character;

    public TextMeshProUGUI nameTxt;
    public SpriteRenderer spriteIcon;
    public Button nextBTN, backBTN, continueBTN;


    private int selectedOption;

    // Start is called before the first frame update
    void Start()
    {
        continueBTN.interactable = false;
        Debug.Log(GlobalVars.choosenPlayers.Count);
        if (PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOption);
    }
    public void NextOption()
    {
        selectedOption = (selectedOption + 1) % characterData.CharacterCount;
        if(selectedOption >= characterData.CharacterCount)
        {
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
        Save();
    }
    public void BackOption()
    {
        selectedOption = (selectedOption - 1) % characterData.CharacterCount;
        if (selectedOption < 0)
        {
            selectedOption = characterData.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
        Save();
    }
    private void UpdateCharacter(int selectedOption)
    {
        character = characterData.GetCharacter(selectedOption);
        spriteIcon.sprite = character.charSprite;
        nameTxt.text = character.charName;
    }
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
    private void Save()
    {
        PlayerPrefs.SetInt("selectedOption", selectedOption);
    }
    public void ChangeScene(string lvlName)
    {
        SceneManager.LoadScene(lvlName);
    }
    public void ConfirmChoice()
    {
        for(int i = 0; i <= selectedOption; i++)
        {
            GlobalVars.choosenPlayers.Add(character.charStats);
            continueBTN.interactable = true;
            nextBTN.interactable = false;
            backBTN.interactable = false;
            Debug.Log(GlobalVars.choosenPlayers.Count);
        }


    }
}
