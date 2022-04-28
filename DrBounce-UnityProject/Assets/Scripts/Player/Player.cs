using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        GameManager.player = this;
        GameManager.SetCursorEnabled(false);
        PauseHandler.SetCanPause(true);

        // Load in pause menu
        if (!SceneManagement.IsSceneLoaded(PauseHandler.pauseMenuSceneName))
        {
            SceneManager.LoadSceneAsync(PauseHandler.pauseMenuSceneName, LoadSceneMode.Additive);
        }
    }

    private void OnDisable()
    {
        // Unload pause menu
        if (SceneManagement.IsSceneLoaded(PauseHandler.pauseMenuSceneName))
        {
            SceneManager.UnloadSceneAsync(PauseHandler.pauseMenuSceneName, UnloadSceneOptions.None);
        }
    }

    public static Player GetPlayer()
    {
        return GameManager.player;
    }

    public static void SetControlsActive(bool active)
    {
        if (active)
        {
            InputManager.inputMaster.Player.Enable();
        }
        else
        {
            InputManager.inputMaster.Player.Disable();
        }
    }
}
