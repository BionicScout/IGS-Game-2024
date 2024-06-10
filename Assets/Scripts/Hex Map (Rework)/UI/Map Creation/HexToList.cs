using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexToList : MonoBehaviour {

    public List<SO_TileInfo> predefinedTiles = new List<SO_TileInfo>();

    void Start() {
        for(int i = 0; i < predefinedTiles.Count; i++) {
            CreateTile(predefinedTiles[i]);
        }
    }

    public void CreateTile(SO_TileInfo tileInfo) {
        GameObject tileObj = new GameObject("Example");
        tileObj.transform.parent = transform;

        Image image = tileObj.AddComponent<Image>();
        image.sprite = tileInfo.sprite;
    }


}
