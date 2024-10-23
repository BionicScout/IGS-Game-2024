using UnityEngine;

public class CombatUI_Rework : MonoBehaviour {
    public void StatsMenuButtonPress() {
        CombatUIEvents.current.flipToggleStatsMenu();
        CombatUIEvents.current.updateStatsMenu(FindAnyObjectByType<InputManager>());
    }

    public void ItemMenuButtonPress(Items items) {
        CombatUIEvents.current.flipToggleItemMenu();
        CombatUIEvents.current.updateItemMenu(items);
    }
}
