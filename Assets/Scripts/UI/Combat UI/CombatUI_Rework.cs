using UnityEngine;

public class CombatUI_Rework : MonoBehaviour {
    public void StatsMenuButtonPress(bool turnOn) {
        Debug.Log("World of HELL");
        CombatUIEvents.current.flipToggleStatsMenu();
        CombatUIEvents.current.updateStatsMenu(FindAnyObjectByType<InputManager>());
    }
}
