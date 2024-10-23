using System;
using UnityEngine;

public class CombatUIEvents : MonoBehaviour {
    public static CombatUIEvents current;

    private void Start() {
        current = this;
    }

    /*********************************
        Selected Unit Menus
    *********************************/

    public event Action<Stats> onCharacterSelect;
    public void updateSelectedCharacter(InputManager inputManager, TurnManager turnManager) {
        if(onCharacterSelect != null) {
            Vector3Int playerCoord = inputManager.getPlayerCoord();
            Stats playerStats = GlobalVars.players[playerCoord];

            onCharacterSelect(playerStats);
            updateTracker(turnManager, playerCoord);
        }
    }

    public event Action<int, bool, bool> onTrackerUpdate;
    public void updateTracker(TurnManager tm, Vector3Int playerPos) {
        if(onTrackerUpdate != null) {
            onTrackerUpdate(tm.getMovementLeft(playerPos), tm.getPotionUse(playerPos) , tm.getActionUse(playerPos));
        }
    }


    /*********************************
        End Turn Banner
    *********************************/
    public event Action<bool> onToggleEndTurnBanner;
    public void toggleEndTurnBanner(bool turnOn) {
        if(onToggleEndTurnBanner != null) {
            onToggleEndTurnBanner(turnOn);
        }
    }

    /*********************************
        Stats Menu
    *********************************/
    public event Action<bool> onToggleStatsMenu;
    public void toggleStatsMenu(bool turnOn) {
        if(onToggleStatsMenu != null) {
            onToggleStatsMenu(turnOn);
        }
    }

    public event Action onFlipToggleStatsMenu;
    public void flipToggleStatsMenu() {
        if(onToggleStatsMenu != null) {
            onFlipToggleStatsMenu();
        }
    }

    public event Action<Stats> onUpdateStatsMenu;
    public void updateStatsMenu(InputManager inputManager) {
        if(onUpdateStatsMenu != null) {
            Vector3Int playerCoord = inputManager.getPlayerCoord();
            Stats playerStats = GlobalVars.players[playerCoord];

            onUpdateStatsMenu(playerStats);
        }
    }

    /*********************************
        Item Menu
    *********************************/
    public event Action<bool> onToggleItemMenu;
    public void toggleItemMenu(bool turnOn) {
        if(onToggleItemMenu != null) {
            onToggleItemMenu(turnOn);
        }
    }

    public event Action onFlipToggleItemMenu;
    public void flipToggleItemMenu() {
        if(onToggleItemMenu != null) {
            onFlipToggleItemMenu();
        }
    }

    public event Action<Items> onUpdateItemMenu;
    public void updateItemMenu(Items items) {
        if(onUpdateItemMenu != null) {
            onUpdateItemMenu(items);
        }
    }

    /*********************************
        Combat Log
    *********************************/
    public event Action<string> onAddCombatLog;
    public void addCombatLog(string message) {
        if(onAddCombatLog != null) {
            onAddCombatLog(message);
        }
    }

    public event Action onCombatLogUp;
    public void CombatLogUp() {
        if(onCombatLogUp != null) {
            onCombatLogUp();
        }
    }

    public event Action onCombatLogDown;
    public void CombatLogDown() {
        if(onCombatLogDown != null) {
            onCombatLogDown();
        }
    }

}