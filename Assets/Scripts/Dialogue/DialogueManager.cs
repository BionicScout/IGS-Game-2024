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
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private GameObject selectedPlayerMenu;
    private static DialogueManager instance;
    private Story currentStory;
    public string nextScene = "CharcaterSelector";
    public bool dialogueIsPlaying { get; private set; }
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

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
    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePannel.SetActive(false);
        dialogueTxt.text = " ";
        selectedPlayerMenu.SetActive(true);
    }
    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //sets dialogue
            dialogueTxt.text = currentStory.Continue();
            //will handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
            GlobalVars.levelClear();
            SceneSwapper.A_LoadScene(nextScene);
        }
    }
    public static DialogueManager GetInstance()
    {
        Debug.Log("Instance was called");
        return instance;
    }

    private void HandleTags(List<string> currentTags)
    {
        //will loop through each tag
        foreach (string tag in currentTags)
        {
            //parse the tags
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.Log("Tag could not be split: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handles the tag
            switch(tagKey)
            {
                case SPEAKER_TAG:
                    nameTxt.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    Debug.Log("Portrait = " + tagValue);
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    Debug.Log("layout = " + tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag cam in but is not being handled " + tag);
                    break;
            }
        }
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