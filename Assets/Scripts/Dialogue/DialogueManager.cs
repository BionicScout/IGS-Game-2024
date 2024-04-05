using Ink.Parsed;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Story = Ink.Runtime.Story;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePannel;
    [SerializeField] private TextMeshProUGUI dialogueTxt;
    [SerializeField] private GameObject selectedPlayerMenu;
    private static DialogueManager instance;
    private Story currentStory;
    public string nextScene = "CharcaterSelector";
    public bool dialogueIsPlaying { get; private set; }

     private void Awake()
    {
        instance = this;
        Debug.Log("Instance was set");
        if (instance == null) 
        {
            Debug.LogWarning("Found more then one Dialogue manager in this scene");
        }
    }
    private void Start()
    {
       //selectedPlayerMenu.SetActive(false);
    }
    private void Update()
    {
        if(!dialogueIsPlaying)
        {
            return;
        }

    }
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePannel.SetActive(true);
        ContinueStory();
    }
    public void ExitDialogueMode()
    {
        //yield return new WaitForSeconds(0.2f);
        //dialogueIsPlaying = false;
        //dialoguePannel.SetActive(false);
        //dialogueTxt.text = " ";
        //selectedPlayerMenu.SetActive(true);
    }
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueTxt.text = currentStory.Continue();
        }
        else
        {
            //StartCoroutine(ExitDialogueMode());
            GlobalVars.levelClear();
            SceneSwapper.A_LoadScene(nextScene);
        }
    }
    public static DialogueManager GetInstance()
    {
        Debug.Log("Instance was called");
        return instance;
    }
}


//    // Start is called before the first frame update
//    void Start()
//    {
//        sentences = new Queue<string>();
//    }
//    public void StartDialogue(Dialogue dialogue)
//    {
//        animator.SetBool("IsOpen", true);
//        nameTxt.text = dialogue.name;
//        sentences.Clear();
//        foreach(string sentence in dialogue.sentences)
//        {
//            sentences.Enqueue(sentence);
//        }
//        DisplayNextSentence();
//    }

//    public void DisplayNextSentence()
//    {
//        if(sentences.Count == 0)
//        {
//            EndDialogue();
//            return;
//        }
//        string sentence = sentences.Dequeue();
//        StopAllCoroutines();
//        StartCoroutine(TypeSentence(sentence));
//    }
//    IEnumerator TypeSentence(string sentence)
//    {
//        dialogueTxt.text = " ";
//        foreach(char letter in sentence.ToCharArray())
//        {
//            dialogueTxt.text += letter;
//            yield return null;
//        }
//    }
//    public void EndDialogue()
//    {
//        animator.SetBool("IsOpen", false);
//        selectedPlayerMenu.SetActive(true);
//        dialogueCanvas.SetActive(false);
//    }