using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneViaScreen(string name)
    {
        LoadingScreenManager.s_Instance.LoadScene(name, LoadingScreenManager.ContinueOptions.RequireInput, LoadingScreenManager.UnloadOptions.Manual, LoadingScreenManager.UnloadOptions.Manual, 1.0f);
    }

    public void LoadLevelViaScreen(LevelData level)
    {
        LoadSceneViaScreen(level.GetSceneName());
    }

    public void ReloadActiveSceneViaScreen()
    {
        LoadSceneViaScreen(SceneManager.GetActiveScene().name);
    }
}