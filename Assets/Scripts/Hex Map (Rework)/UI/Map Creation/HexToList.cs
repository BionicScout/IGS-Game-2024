using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexToList : MonoBehaviour {

    public List<SO_TileInfo> predefinedTiles = new List<SO_TileInfo>();
    private Input_MapSelection inputScript; 

    void Start() {
        inputScript = FindAnyObjectByType<Input_MapSelection>();

        for(int i = 0; i < predefinedTiles.Count; i++) {
            CreateTile(predefinedTiles[i]);
        }
    }

    public GameObject CreateTile(SO_TileInfo tileInfo) {
        // Create a new GameObject for the tile
        GameObject tileObj = new GameObject("TileObject");
        tileObj.transform.parent = transform;

        // Add an Image component and set its sprite
        Image image = tileObj.AddComponent<Image>();
        image.sprite = tileInfo.sprite;

        // Add a HexUIList component to store data
        HexUIList hexUIList = tileObj.AddComponent<HexUIList>();
        hexUIList.type = "HexBase";
        hexUIList.data = tileInfo;

        // Add an EventTrigger component to handle events
        EventTrigger trigger = tileObj.AddComponent<EventTrigger>();

        // Add a pointer click event listener
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick; // Set the event type to click

        // Create a new UnityAction to handle the click event
        UnityAction<BaseEventData> clickAction = new UnityAction<BaseEventData>((data) => {
            Debug.Log("Tile clicked: " + tileInfo.name); // Example action, replace with your logic
            inputScript.HandleTileClick(tileObj); // Pass the tileObj to handle click logic
        });

        // Add the listener to the trigger
        entry.callback.AddListener((eventData) => clickAction.Invoke(eventData));
        trigger.triggers.Add(entry);

        return tileObj; // Return the created tile object
    }
}
