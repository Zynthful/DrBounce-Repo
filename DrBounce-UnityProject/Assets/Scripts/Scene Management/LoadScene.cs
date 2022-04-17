using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneViaScreen(string name)
    {
        LoadingScreenManager.s_Instance.LoadScene(
            name,
            LoadingScreenManager.ContinueOptions.RequireInput,
            LoadingScreenManager.UnloadOptions.Manual,
            LoadingScreenManager.UnloadOptions.Manual,
            1.2f);
    }

    public void LoadLevelViaScreen(LevelData level)
    {
        LoadSceneViaScreen(level.GetSceneName());
    }

    public void ReloadActiveScene(bool useLoadingScreen)
    {
        if (useLoadingScreen)
            LoadSceneViaScreen(SceneManager.GetActiveScene().name);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}