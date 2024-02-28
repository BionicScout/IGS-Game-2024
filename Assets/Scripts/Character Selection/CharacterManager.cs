using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    public CharacterData characterData;

    public TextMeshProUGUI nameTxt;
    public SpriteRenderer spriteIcon;

    private int selectedOption;

    // Start is called before the first frame update
    void Start()
    {
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
        selectedOption++;
        if(selectedOption >= characterData.CharacterCount)
        {
            selectedOption = 0;
        }
        UpdateCharacter(selectedOption);
        Save();
    }
    public void BackOption()
    {
        selectedOption--;
        if(selectedOption < 0)
        {
            selectedOption = characterData.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
        Save();
    }
    private void UpdateCharacter(int selectedOption)
    {
        Characters character = characterData.GetCharacter(selectedOption);
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
    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
