using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterData characterData;

    public TextMeshProUGUI nameTxt;
    public SpriteRenderer spriteIcon;

    private int selectedOption;

    // Start is called before the first frame update
    void Start()
    {
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
    }
    public void BackOption()
    {
        selectedOption--;
        if(selectedOption < 0)
        {
            selectedOption = characterData.CharacterCount - 1;
        }
        UpdateCharacter(selectedOption);
    }
    private void UpdateCharacter(int selectedOption)
    {
        Characters character = characterData.GetCharacter(selectedOption);
        spriteIcon.sprite = character.charSprite;
        nameTxt.text = character.charName;
    }
}
