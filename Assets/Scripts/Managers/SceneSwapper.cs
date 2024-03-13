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
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }

    public static string GetCurrentScene() {
        return currentScene;
    }

    public static void LoadHoldScene() {
        A_LoadScene(holdLoadingScene);
    }
}
