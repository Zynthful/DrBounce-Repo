using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;

    public InputMaster controls;

    [Header("Events")]
    [SerializeField]
    private GameEventBool onPauseStateChange = null;
    [SerializeField]
    private GameEvent onPause = null;
    [SerializeField]
    private GameEvent onUnpause = null;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Menu.Pause.performed += _ => Pause();
    }

    public void Pause()
    {
        paused = !paused;

        Time.timeScale = paused ? 0 : 1;

        // Update cursor lock state and visibility
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;

        // Raise events
        onPauseStateChange?.Raise(paused);
        if (paused)
        {
            onPause?.Raise();
        }
        else
        {
            onUnpause?.Raise();
        }
    }

    public void LoadScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public void QuitToDesktop()
    {
        Time.timeScale = 1;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Pause.performed -= _ => Pause();
        controls.Disable();
    }
}
