using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleCharacterSelection : MonoBehaviour {
    public TextMeshProUGUI nameTxt;
    SpriteRenderer spriteIcon;
    public Button nextBTN, backBTN;
    public SpriteRenderer previousChar, nextChar;

    public int selectedOption = 0;

    UnitSelectionManager selMan;

    private void Start() {
        spriteIcon = GetComponent<SpriteRenderer>();
    }
    //buttons that change the selection number
    public void NextOption() {
        selectedOption = (selectedOption + 1) % selMan.playerStats.Count;
        if(selectedOption >= selMan.playerStats.Count) {
            selectedOption = 0;
        }
        UpdateCharacter();
    }

    public void BackOption() {
        selectedOption = (selectedOption - 1) % selMan.playerStats.Count;
        if(selectedOption < 0) {
            selectedOption = selMan.playerStats.Count - 1;
        }
        UpdateCharacter();
    }
    //changes the name and sprite depending on selection number
    private void UpdateCharacter() {
        Stats stats = selMan.playerStats[selectedOption];
        spriteIcon.sprite = stats.sprite;
        nameTxt.text = stats.charType;

        //PreviousCharacter();
        int prevChar = (selectedOption - 1) % selMan.playerStats.Count;
        if (prevChar <= 0)
        {
            prevChar = selMan.playerStats.Count - 1;
        }
        stats = selMan.playerStats[prevChar];
        previousChar.sprite = stats.sprite;
        //NextCharacter();        
        int nxtChar = selectedOption + 1;
        if (nxtChar >= selMan.playerStats.Count)
        {
            nxtChar = 0;
        }
        stats = selMan.playerStats[nxtChar];
        nextChar.sprite = stats.sprite;

    }
    //functions to load the chossen characters into the levels
    public void Load(UnitSelectionManager manager, int loadChar) {
        selMan = manager;

        selectedOption = loadChar;
        UpdateCharacter();
    }
}
