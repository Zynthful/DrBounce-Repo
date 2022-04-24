/// This script is used to pause and unpause the game via additively loading/unloading a Pause Menu scene

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseHandler : MonoBehaviour
{
    private static bool hasPausedOnce = false;         // Has the player paused at least once since initialisation?
    private static bool disabledControls = false;      // So we only re-enable controls if we disabled them
    private static bool pausingOrUnpausing = false;    // So we don't pause/unpause whilst already doing so

    /// <summary>
    /// Use to prevent/allow pausing.
    /// </summary>
    private static bool canPause = true;
    public static bool GetCanPause() { return canPause; }
    public static void SetCanPause(bool value) { canPause = value; }

    [SerializeField]
    [Tooltip("MUST match the name of the Pause Menu scene to load when pausing.")]
    private string pauseMenuSceneName = "PauseMenu_SCN";

    [Header("Events")]
    public UnityEvent onPauseBegin = null;      // Begin and complete events currently happen at the same time
    public UnityEvent onPauseComplete = null;   // but if we wanted to have a transition for pausing (such as slowing the game down), this would be useful for it
    public UnityEvent onUnpauseBegin = null;
    public UnityEvent onUnpauseComplete = null;
    public UnityEvent onUnpauseAfterPauseBegin = null;
    public UnityEvent onUnpauseAfterPauseComplete = null;
    public UnityEvent<bool> onIsPaused = null;

    private void OnEnable()
    {
        InputManager.inputMaster.Menu.Pause.performed += _ => InvertPause();
    }

    private void Awake()
    {
        // Load in pause menu
        if (!SceneManagement.IsSceneLoaded(pauseMenuSceneName) && GameManager.player != null)
        {
            SceneManager.LoadSceneAsync(pauseMenuSceneName, LoadSceneMode.Additive);
        }
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Menu.Pause.performed -= _ => InvertPause();

        // Unload pause menu
        if (SceneManagement.IsSceneLoaded(pauseMenuSceneName))
        {
            SceneManager.UnloadSceneAsync(pauseMenuSceneName, UnloadSceneOptions.None);
        }
    }

    private void InvertPause()
    {
        SetPaused(!GameManager.s_Instance.paused);
    }

    public void SetPaused(bool value)
    {
        if (pausingOrUnpausing || !canPause || value == GameManager.s_Instance.paused || Player.GetPlayer() == null)
            return;

        pausingOrUnpausing = true;

        GameManager.s_Instance.paused = value;
        onIsPaused.Invoke(value);

        if (value)
        {
            onPauseBegin.Invoke();
            onPauseComplete.Invoke();
            hasPausedOnce = true;
        }
        else
        {
            onUnpauseBegin.Invoke();
            onUnpauseComplete.Invoke();

            if (hasPausedOnce)
            {
                onUnpauseAfterPauseBegin.Invoke();
                onUnpauseAfterPauseComplete.Invoke();
            }
        }

        // Disables player controls if they were enabled and we're pausing
        if (InputManager.inputMaster.Player.enabled && value)
        {
            InputManager.SetActionMapActive(InputManager.inputMaster.Player, false);
            disabledControls = true;
        }
        // Re-enables player controls if we disabled them and we're unpausing
        else if (disabledControls && !value)
        {
            InputManager.SetActionMapActive(InputManager.inputMaster.Player, true);
            disabledControls = false;
        }

        // Update cursor lock state and visibility
        Cursor.lockState = value || GameManager.player == null ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value || GameManager.player == null;

        pausingOrUnpausing = false;
        
        // ASYNC PAUSING AND UNPAUSING (was very laggy when pausing/unpausing)
        /*
        AsyncOperation operation = null;
        if (value)
        {
            onPauseBegin.Invoke();
            hasPausedOnce = true;
            operation = SceneManager.LoadSceneAsync(pauseMenuSceneName, LoadSceneMode.Additive);
            operation.completed += _ =>
            {
                onPauseComplete.Invoke();
            };
        }
        else
        {
            onUnpauseBegin.Invoke();
            if (hasPausedOnce)
                onUnpauseAfterPauseBegin.Invoke();

            if (SceneManagement.IsSceneLoaded(pauseMenuSceneName))
            {
                operation = SceneManager.UnloadSceneAsync(pauseMenuSceneName, UnloadSceneOptions.None);
                operation.completed += _ =>
                {
                    onUnpauseComplete.Invoke();

                    if (hasPausedOnce)
                        onUnpauseAfterPauseComplete.Invoke();
                };
            }
        }

        if (operation != null)
        {
            operation.completed += _ =>
            {
                GameManager.s_Instance.paused = value;
                onIsPaused.Invoke(value);
                pausingOrUnpausing = false;
            };
        }
        else
        {
            GameManager.s_Instance.paused = value;
            onIsPaused.Invoke(value);
            pausingOrUnpausing = false;
        }
        */
    }
}