using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseScreen : MonoBehaviour
{
    public string nextScene;
    public GameObject winMenu;
    public GameObject loseMenu;
    public void Replay()
    {
        SceneSwapper.A_LoadScene(SceneSwapper.currentScene);
    }
    public void Quit()
    {
        SceneSwapper.A_LoadScene("MainMenu");
    }
    public void Continue()
    {
        SceneSwapper.A_LoadScene(nextScene);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
    }
}
