using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : MonoBehaviour {
    Image selectedUnitImage;
    GameObject tracker_movement,tracker_potion, tracker_action;

    private void Start() {
        CombatUIEvents.current.onCharacterSelect += UpdatePortrait;
        CombatUIEvents.current.onTrackerUpdate += UpdateTracker;

        selectedUnitImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();

        tracker_movement = transform.GetChild(1).GetChild(0).gameObject;
        tracker_potion = transform.GetChild(1).GetChild(1).gameObject;
        tracker_action = transform.GetChild(1).GetChild(2).gameObject;
    }

    private void OnDestroy() {
        CombatUIEvents.current.onCharacterSelect -= UpdatePortrait;
        CombatUIEvents.current.onTrackerUpdate -= UpdateTracker;
    }

    private void UpdatePortrait(Stats PlayerStats) {
        selectedUnitImage.sprite = PlayerStats.squareSprite;
        CombatUIEvents.current.toggleStatsMenu(false);
    }

    private void UpdateTracker(int movementLeft, bool potionUsed, bool actionUsed) {
        //All available
        if(movementLeft > 0) {
            tracker_movement.SetActive(true);
            tracker_potion.SetActive(true);
            tracker_action.SetActive(true);
            return;
        }

        //Movement Used, Potion + Action Available
        if(!potionUsed) {
            tracker_movement.SetActive(false);
            tracker_potion.SetActive(true);
            tracker_action.SetActive(true);
            return;
        }

        //Movement + Potion Used, Action Available
        if(!actionUsed) {
            tracker_movement.SetActive(false);
            tracker_potion.SetActive(false);
            tracker_action.SetActive(true);
            return;
        }

        tracker_movement.SetActive(false);
        tracker_potion.SetActive(false);
        tracker_action.SetActive(false);
        return;
    }
}
