using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField] private TextAsset inkJSON;

    public void TriggerDialogue() 
    { 
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    private void Start()
    {
        Debug.Log("Called Instance");
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }

}
