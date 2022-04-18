/// This script is used to pause and unpause the game via additively loading/unloading a Pause Menu scene

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseHandler : MonoBehaviour
{
    private bool hasPausedOnce = false; // Has the player paused at least once since initialisation?
    private bool disabledControls = false;

    private bool canPause = true;
    public bool GetCanPause() { return canPause; }
    public void SetCanPause(bool value) { canPause = value; }

    [Header("Events")]
    public UnityEvent onPauseBegin = null;
    public UnityEvent onPauseComplete = null;
    public UnityEvent onUnpauseBegin = null;
    public UnityEvent onUnpauseComplete = null;
    public UnityEvent onUnpauseAfterPauseBegin = null;
    public UnityEvent onUnpauseAfterPauseComplete = null;
    public UnityEvent<bool> onIsPaused = null;

    private void OnEnable()
    {
        InputManager.inputMaster.Menu.Pause.performed += _ => InvertPause();
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Menu.Pause.performed -= _ => InvertPause();
    }

    private void InvertPause()
    {
        SetPaused(!GameManager.s_Instance.paused);
    }

    public void SetPaused(bool value)
    {
        if (!canPause || Player.GetPlayer() == null)
            return;

        GameManager.s_Instance.paused = value;

        // To ensure we only re-enable controls if we were the one to disable them:
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
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;

        if (value)
        {
            onPauseBegin.Invoke();
            hasPausedOnce = true;
            AsyncOperation load = SceneManager.LoadSceneAsync("PauseMenu_SCN", LoadSceneMode.Additive);
            load.completed += _ => onPauseComplete.Invoke();
        }
        else
        {
            onUnpauseBegin.Invoke();
            if (hasPausedOnce)
                onUnpauseAfterPauseBegin.Invoke();

            if (SceneManagement.IsSceneLoaded("PauseMenu_SCN"))
            {
                AsyncOperation unload = SceneManager.UnloadSceneAsync("PauseMenu_SCN", UnloadSceneOptions.None);
                unload.completed += _ =>
                {
                    onUnpauseComplete.Invoke();

                    if (hasPausedOnce)
                        onUnpauseAfterPauseComplete.Invoke();
                };
            }
        }
    }
}