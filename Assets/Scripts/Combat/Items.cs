using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int singleHealAMT, powerBuffAMT, defenseBuffAMT, reviveAMT, healScrollAMT;
    public TextMeshProUGUI singleHealTxt, powerBuffTxt, defenseBuffTxt, reviveTxt, healScrollTxt;
    public Button singleHealBTN, powerBuffBTN, defenseBuffBTN, reviveBTN, healScrollBTN;
    public Button shUseBTN, pbUseBTN, dbUseBTN, rUseBTN, hsUSeBTN;

    // Start is called before the first frame update
    void Start()
    {
        //singleHealAMT = 0;
        //powerBuffAMT = 0;
        //defenseBuffAMT = 0;
        //reviveAMT = 0;
        //healScrollAMT = 0;

    }
    //For buttons
    public void SetSingleHeal()
    {
        shUseBTN.gameObject.SetActive(true);
        shUseBTN.interactable = true;
    }
    public void SetPowerBuff()
    {
        pbUseBTN.gameObject.SetActive(true);
        pbUseBTN.interactable = true;
    }
    public void SetDefenseBuff()
    {
        dbUseBTN.gameObject.SetActive(true);
        dbUseBTN.interactable = true;
    }
    public void SetRevive()
    {
        rUseBTN.gameObject.SetActive(true);
        rUseBTN.interactable = true;
    }
    public void SetHealScroll()
    {
        hsUSeBTN.gameObject.SetActive(true);
        hsUSeBTN.interactable = true;
    }

    public void ClearItems()
    {
        singleHealAMT = 0;
        powerBuffAMT = 0;
        defenseBuffAMT = 0;
        reviveAMT = 0;
        healScrollAMT = 0;
    }
}
