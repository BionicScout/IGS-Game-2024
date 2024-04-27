using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseScreen : MonoBehaviour
{
    public string nextScene;

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
        string currentLevel = SceneSwapper.currentScene;
        int levelNum = int.Parse(currentLevel.Substring(currentLevel.Length - 1));
        levelNum++;
        string nextLevel = "Level " + levelNum.ToString();



        SceneSwapper.holdLoadingScene = nextLevel;
        SceneSwapper.A_LoadScene("LevelingUp");

    }
}
