using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitToDesktop()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void QuitToMainMenu()
    {
        LoadingScreenManager.s_Instance.LoadScene(
            "LabMainMenu_SCN",
            LoadingScreenManager.ContinueOptions.RequireInput,
            LoadingScreenManager.UnloadOptions.Manual,
            LoadingScreenManager.UnloadOptions.Manual,
            1.2f);
    }
}
