using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSwapper {
    public static string currentScene;
    public static string holdLoadingScene;

    public static void setCurrentScene() {
        currentScene = SceneManager.GetActiveScene().name;
        //Debug.Log(currentScene);
    }

    public static void A_ExitButton() {
        Application.Quit();
    }

    public static void A_LoadScene(string sceneName) {
        //Switch Scene
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;

        if(currentScene == "MainMenu")
            AudioManager.instance.PlayFromSoundtrack("Menu-Music");
        if(currentScene == "Tutorial" || currentScene == "Level 1")
            AudioManager.instance.PlayFromSoundtrack("Combat Music");


    }

    public static string GetCurrentScene() {
        return currentScene;
    }

    public static void LoadHoldScene() {
        if(holdLoadingScene == null) {
            Debug.LogError("SceneSwapper - HoldLoadingScene is null");
        }

        A_LoadScene(holdLoadingScene);
        holdLoadingScene = null;
    }
}
