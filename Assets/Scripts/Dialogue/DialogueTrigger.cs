using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TurnManager turnManager;
    [SerializeField] public TextAsset intro;
    [SerializeField] public TextAsset outro;

    private void Start()
    {
        Debug.Log("Called Instance");
        DialogueManager.GetInstance().EnterDialogueMode(intro);
    }

    //public void Update()
    //{
    //    if (turnManager.winConMet)
    //    {
    //        DialogueManager.GetInstance().EnterDialogueMode(outro);
    //    }
    //}

}
