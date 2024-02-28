using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavedPlayers : MonoBehaviour
{
    public CharacterData characterData;
    public SpriteRenderer spriteIcon;

    private int selectedOption = 0;
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
    private void UpdateCharacter(int selectedOption)
    {
        Characters character = characterData.GetCharacter(selectedOption);
        spriteIcon.sprite = character.charSprite;
    }
    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
