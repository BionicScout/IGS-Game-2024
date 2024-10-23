using TMPro;
using UnityEditor;
using UnityEngine;

public class ToggleableUI : MonoBehaviour {
    GameObject statsMenu, itemMenu, endTurnBanner;

    private void Start() {
        endTurnBanner = transform.parent.GetChild(3).gameObject;
        statsMenu = transform.parent.GetChild(6).gameObject;
        itemMenu = transform.parent.GetChild(7).gameObject;

        CombatUIEvents.current.onToggleStatsMenu += ToggleStatsMenu;
        CombatUIEvents.current.onUpdateStatsMenu += updateStatsMenu;
        CombatUIEvents.current.onFlipToggleStatsMenu += flipToggleStatsMenu;

        CombatUIEvents.current.onToggleItemMenu += ToggleItemMenu;
        CombatUIEvents.current.onUpdateItemMenu += updateItemMenu;
        CombatUIEvents.current.onFlipToggleItemMenu += flipToggleItemMenu;

        CombatUIEvents.current.onToggleEndTurnBanner += ToggleEndTurnBanner;
    }

    private void OnDestroy() {
        CombatUIEvents.current.onToggleStatsMenu -= ToggleStatsMenu;
        CombatUIEvents.current.onUpdateStatsMenu -= updateStatsMenu;
        CombatUIEvents.current.onFlipToggleStatsMenu -= flipToggleStatsMenu;

        CombatUIEvents.current.onToggleItemMenu -= ToggleItemMenu;
        CombatUIEvents.current.onUpdateItemMenu -= updateItemMenu;
        CombatUIEvents.current.onFlipToggleItemMenu -= flipToggleItemMenu;

        CombatUIEvents.current.onToggleEndTurnBanner -= ToggleEndTurnBanner;
    }

    /*********************************
        End Turn Banner
    *********************************/
    private void ToggleEndTurnBanner(bool turnOn) {
        endTurnBanner.SetActive(turnOn);
    }

    /*********************************
        Stats Menu
    *********************************/
    private void ToggleStatsMenu(bool turnOn) {
        statsMenu.SetActive(turnOn);
    }

    private void flipToggleStatsMenu() {
        ToggleStatsMenu(!statsMenu.activeSelf);
    }

    public void updateStatsMenu(Stats unitStats) {
        Transform baseObj = statsMenu.transform.GetChild(0);

        //Update Name, Level, and Type
        baseObj.GetChild(0).GetComponent<TMP_Text>().text = unitStats.charType.ToString();
        baseObj.GetChild(1).GetComponent<TMP_Text>().text = "Level: " + unitStats.charLevel.ToString();
        baseObj.GetChild(2).GetComponent<TMP_Text>().text = "Type: " + unitStats.charName;

        //Update Stats
        baseObj = baseObj.GetChild(4);

        baseObj.GetChild(1).GetComponent<TMP_Text>().text = unitStats.move.ToString();
        baseObj.GetChild(0).GetComponent<TMP_Text>().text = unitStats.power.ToString();
        baseObj.GetChild(3).GetComponent<TMP_Text>().text = unitStats.defense.ToString();
        baseObj.GetChild(4).GetComponent<TMP_Text>().text = unitStats.maxHealth.ToString();
        baseObj.GetChild(2).GetComponent<TMP_Text>().text = unitStats.attackRange.ToString();
    }

    /*********************************
        Item Menu
    *********************************/
    private void ToggleItemMenu(bool turnOn) {
        itemMenu.SetActive(turnOn);
    }

    private void flipToggleItemMenu() {
        itemMenu.SetActive(!itemMenu.activeSelf);
    }

    private void updateItemMenu(Items items) {
        items.singleHealTxt.text = "x" + items.singleHealAMT.ToString();
        items.powerBuffTxt.text = "x" + items.powerBuffAMT.ToString();
        items.defenseBuffTxt.text = "x" + items.defenseBuffAMT.ToString();
        items.healScrollTxt.text = "x" + items.healScrollAMT.ToString();

        Debug.Log("ERROR[ToggleableUI]: Need implimentation for \"updateItemMenu()\"");
    }

}
