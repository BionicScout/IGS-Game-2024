using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] public TextAsset intro;
    [SerializeField] public TextAsset outro;

    public void TriggerDialogue() 
    { 
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    private void Start()
    {
        Debug.Log("Called Instance");
        DialogueManager.GetInstance().EnterDialogueMode(intro);
    }

}
