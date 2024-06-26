using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour {
    public Vector3Int currentHex;
    public Vector3Int goalHex;

    public int range = 5;

    public Rigidbody2D rb;

    public Sprite playerSprite;

    private void Start() {
        currentHex = GlobalVars.centerHex;

        GlobalVars.hexagonTile[currentHex].transform.GetComponent<HexObjInfo>().unitName = "Player";
        GlobalVars.hexagonTile[currentHex].transform.GetComponent<HexObjInfo>().unitSprite = playerSprite;
        GlobalVars.hexagonTile[currentHex].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = playerSprite;
    }



    public static void movePlayer(Vector3Int playerCoord, Vector3Int newTileCoord)
    {
        //Get Player and current + future hex objs
        Stats playerStats = GlobalVars.players[playerCoord];
        GameObject currentTileObj = GlobalVars.hexagonTile[playerCoord];
        GameObject newTileObj = GlobalVars.hexagonTile[newTileCoord];

        //Update Sprite
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
        newTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = playerStats.sprite;

        //Update player coord
        GlobalVars.players.Remove(playerCoord);
        GlobalVars.players.Add(newTileCoord, playerStats);

        //Update Health Bar
        currentTileObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        newTileObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        newTileObj.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = playerStats.curHealth / playerStats.maxHealth;

        Vector3 offset = newTileObj.transform.position - currentTileObj.transform.position;
        //playerStats.healthBar.transform.position = playerStats.healthBar.transform.position + offset;
    }

    public static void moveEnemy(Vector3Int enemyCoord , Vector3Int newTileCoord) {
        //Get Player and current + future hex objs
        Stats enemyStats = GlobalVars.enemies[enemyCoord];
        GameObject currentTileObj = GlobalVars.hexagonTile[enemyCoord];
        GameObject newTileObj = GlobalVars.hexagonTile[newTileCoord];

        //Update Sprite
        currentTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = null;
        newTileObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = enemyStats.sprite;

        //Update Health Bar
        currentTileObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        newTileObj.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        newTileObj.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value = enemyStats.curHealth / enemyStats.maxHealth;

        //Update player coord
        GlobalVars.enemies.Add(newTileCoord , enemyStats);
        GlobalVars.enemies.Remove(enemyCoord);

    }
}
