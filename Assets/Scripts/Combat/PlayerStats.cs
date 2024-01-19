using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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

    //handels pulling up the actions/info menu when a player is clicked on
    public GameObject CharMenu;
    public bool isShown;

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
        }
    }
}
