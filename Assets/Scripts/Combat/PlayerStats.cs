using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour

{   //handels pulling up the actions/info menu when a player is clicked on
    public GameObject CharMenu;
    public bool isShown;
    //all stats for a player
    public int move;
    public int health = 10;
    public bool didDodge;
    public float dodgeChance;
    public int defense;
    public int power;

    //GameObject allCharMenus = GameObject.FindGameObjectWithTag("CharMenu");

    // Start is called before the first frame update
    void Start()
    {
        //setting the characters menu to false
        CharMenu.SetActive(false);
        isShown = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (isShown)
        {
            CharMenu.SetActive(false);
            isShown = false;
        }
        else
        {
            CharMenu.SetActive(true);
            isShown = true;
            //allCharMenus.SetActive(false);
        }
    }
}
