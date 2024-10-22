using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playScene, tutorialScene;

    //functions for buttons
    public void Play()
    {
        SceneSwapper.A_LoadScene(playScene);
    }
    public void PlayTutorial()
    {
        SceneSwapper.A_LoadScene(tutorialScene);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();

    }
}
