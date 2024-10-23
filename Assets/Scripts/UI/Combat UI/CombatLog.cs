using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLog : MonoBehaviour {
    TMP_Text topMessage, middleMessage, bottomMessage;

    List<string> messages = new List<string>(); //Lower indexes are at bottom
    int messageID = 0; //Goes from 0-message.count-3

    private void Start() {
        topMessage = transform.GetChild(0).GetComponent<TMP_Text>();
        middleMessage = transform.GetChild(1).GetComponent<TMP_Text>();
        bottomMessage = transform.GetChild(2).GetComponent<TMP_Text>();

        CombatUIEvents.current.onAddCombatLog += addMessage;
        CombatUIEvents.current.onCombatLogUp += moveUp;
        CombatUIEvents.current.onCombatLogDown += moveDown;
    }

    private void OnDestroy() {
        CombatUIEvents.current.onAddCombatLog -= addMessage;
        CombatUIEvents.current.onCombatLogUp -= moveUp;
        CombatUIEvents.current.onCombatLogDown -= moveDown;
    }

    public void moveUp() {
        if(messages.Count <= 3) { return; }

        messageID++;
        if(messageID > messages.Count - 3) {
            messageID = messages.Count - 3;
        }

        updateCurrentSelection();
    }

    public void moveDown() {
        if(messages.Count <= 3) { return; }

        messageID--;
        if(messageID < 0) {
            messageID = 0;
        }

        updateCurrentSelection();
    }

    public void updateCurrentSelection() {
        if(messages.Count == 0) {
            topMessage.text = "";
            middleMessage.text = "";
            bottomMessage.text = "";
            return; 
        }

        if(messages.Count == 1) {
            topMessage.text = messages[messageID];
            middleMessage.text = "";
            bottomMessage.text = "";
            return;
        }

        if(messages.Count == 2) {
            topMessage.text = messages[messageID + 1];
            middleMessage.text = messages[messageID];
            bottomMessage.text = "";
            return;
        }



        topMessage.text = messages[messageID + 2];
        middleMessage.text = messages[messageID + 1];
        bottomMessage.text = messages[messageID];
    }


    public void addMessage(string message) {
        messages.Add(message);

        messageID = messages.Count - 3;
        if(messageID < 0) {
            messageID = 0;
        }

        updateCurrentSelection();
    }
}
