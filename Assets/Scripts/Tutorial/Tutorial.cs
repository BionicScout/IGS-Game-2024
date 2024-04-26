using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private int lessonNum = 1;
    [Header("Tutorial Lessons")]
    [SerializeField] private TextAsset less1;
    [SerializeField] private TextAsset less2;
    [SerializeField] private TextAsset less3;
    [SerializeField] private TextAsset less4;
    [SerializeField] private TextAsset less5;
    [SerializeField] private TextAsset less6;
    [SerializeField] private TextAsset less7;
    [Header("Character Screen Buttons")]
    public Button statsBTN;
    public Button moveBTN;
    public Button attackBTN;
    public Button interactBTN;
    public Button itemsBTN;

    public void Start()
    {
        DialogueManager.GetInstance().EnterDialogueMode(less1);
        lessonNum += 1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LessonPlayer()
    {
        if (lessonNum == 2)
        {
            Movement.moveEnemy(new Vector3Int(2, 11, -13), new Vector3Int(4, 8, -12));
            DialogueManager.GetInstance().EnterDialogueMode(less2);
            lessonNum += 1;
        }
        if (lessonNum == 3)
        {
            //Movement.moveEnemy(new Vector3Int(2, 11, -13), new Vector3Int(4, 8, -12));
            DialogueManager.GetInstance().EnterDialogueMode(less3);
            lessonNum += 1;
        }
        if (lessonNum == 4)
        {
            //Movement.moveEnemy(new Vector3Int(2, 11, -13), new Vector3Int(4, 8, -12));
            DialogueManager.GetInstance().EnterDialogueMode(less4);
            lessonNum += 1;
        }
        if (lessonNum == 5)
        {
            //Movement.moveEnemy(new Vector3Int(2, 11, -13), new Vector3Int(4, 8, -12));
            DialogueManager.GetInstance().EnterDialogueMode(less5);
            lessonNum += 1;
        }
        if (lessonNum == 6)
        {
            //Movement.moveEnemy(new Vector3Int(2, 11, -13), new Vector3Int(4, 8, -12));
            DialogueManager.GetInstance().EnterDialogueMode(less6);
            lessonNum += 1;
        }
        if (lessonNum == 7)
        {
            //Movement.moveEnemy(new Vector3Int(2, 11, -13), new Vector3Int(4, 8, -12));
            DialogueManager.GetInstance().EnterDialogueMode(less7);
            lessonNum += 1;
        }
    }
    public void ActionManager()
    {
        if(lessonNum == 2)
        {
            statsBTN.interactable = false;
            attackBTN.interactable = false;
            interactBTN.interactable = false;   
            itemsBTN.interactable = false;
        }
        else if(lessonNum == 3) 
        {
            statsBTN.interactable = true;
            attackBTN.interactable = true;
            interactBTN.interactable = false;
            itemsBTN.interactable = false;
        }
        else if (lessonNum == 4)
        {
            statsBTN.interactable = true;
            attackBTN.interactable = true;
            interactBTN.interactable = false;
            itemsBTN.interactable = true;
        }
        else if (lessonNum == 5)
        {
            statsBTN.interactable = true;
            attackBTN.interactable = true;
            interactBTN.interactable = false;
            itemsBTN.interactable = true;
        }
        else if (lessonNum == 6)
        {
            statsBTN.interactable = true;
            attackBTN.interactable = true;
            interactBTN.interactable = false;
            itemsBTN.interactable = true;
        }
        else if (lessonNum == 7)
        {
            statsBTN.interactable = true;
            attackBTN.interactable = true;
            interactBTN.interactable = true;
            itemsBTN.interactable = true;
        }
        else
        {
            return;
        }
    }
}
