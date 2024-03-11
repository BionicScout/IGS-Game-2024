using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleCharacterSelection : MonoBehaviour {
    public TextMeshProUGUI nameTxt;
    SpriteRenderer spriteIcon;
    public Button nextBTN, backBTN;

    public int selectedOption = 0;

    UnitSelectionManager selMan;

    private void Start() {
        spriteIcon = GetComponent<SpriteRenderer>();
    }

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

    private void UpdateCharacter() {
        Stats stats = selMan.playerStats[selectedOption];
        spriteIcon.sprite = stats.sprite;
        nameTxt.text = stats.charName;
    }

    public void Load(UnitSelectionManager manager, int loadChar) {
        selMan = manager;

        selectedOption = loadChar;
        UpdateCharacter();
    }
}
