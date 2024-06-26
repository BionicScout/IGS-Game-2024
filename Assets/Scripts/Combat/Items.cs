using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public bool powerBuffUsed;
    public bool defenseBuffUsed;
    [Header("Amount")]
    public int singleHealAMT;
    public int powerBuffAMT;
    public int defenseBuffAMT;
    public int healScrollAMT;

    [Header("The Text")]
    public TextMeshProUGUI singleHealTxt;
    public TextMeshProUGUI powerBuffTxt;
    public TextMeshProUGUI defenseBuffTxt;
    public TextMeshProUGUI healScrollTxt;

    [Header("Buttons on Item")]
    public Button singleHealBTN;
    public Button powerBuffBTN;
    public Button defenseBuffBTN;
    public Button healScrollBTN;

    [Header("Buttons to Use")]
    public Button shUseBTN;
    public Button pbUseBTN;
    public Button dbUseBTN;
    public Button hsUSeBTN;

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
    public void SetHealScroll()
    {
        hsUSeBTN.gameObject.SetActive(true);
        hsUSeBTN.interactable = true;
    }
    //function to clear inventory/items
    public void ClearItems()
    {
        singleHealAMT = 0;
        powerBuffAMT = 0;
        defenseBuffAMT = 0;
        healScrollAMT = 0;
    }
}
