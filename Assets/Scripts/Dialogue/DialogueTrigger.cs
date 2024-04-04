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
    private void Awake()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }

}
