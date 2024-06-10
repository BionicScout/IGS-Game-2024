using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour {
    public List<TabButton> tabButtons;
    public Sprite tabIdle, tabHover, tabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;

    public void Subscribe(TabButton button) {
        if(tabButtons == null) {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button) {
        ResetTabs();
        if(selectedTab == null || button != selectedTab) {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button) {
        ResetTabs();
        button.background.sprite = tabActive;
    }

    public void OnTabSelected(TabButton button) {
        if(selectedTab != null) { 
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        int index = button.transform.GetSiblingIndex();

        for(int i = 0; i<objectsToSwap.Count; i++) {
            if(i == index) {
                objectsToSwap[i].SetActive(true);
            }
            else {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs() {
        foreach(TabButton button in tabButtons) {
            if(selectedTab != null && selectedTab == button) {
                continue;
            }
            button.background.sprite = tabIdle;
        }
    }
}
