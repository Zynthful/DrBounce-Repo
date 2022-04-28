using UnityEngine;
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

    // MUST match the name of the Pause Menu scene to load
    public static string pauseMenuSceneName = "PauseMenu_SCN";

    [SerializeField]
    private MoreMountains.Feedbacks.MMFeedbacks freezeFeedback = null;

    [Header("Pause Events")]
    public UnityEvent onPauseBegin = null;      // Begin and complete events currently happen at the same time
    public UnityEvent onPauseComplete = null;   // but if we wanted to have a transition for pausing (such as slowing the game down), this would be useful for it
    public UnityEvent onUnpauseBegin = null;
    public UnityEvent onUnpauseComplete = null;
    public UnityEvent onUnpauseAfterPauseBegin = null;
    public UnityEvent onUnpauseAfterPauseComplete = null;
    public UnityEvent<bool> onIsPaused = null;

    [Header("Time Freeze Events")]
    public UnityEvent onFreeze = null;
    public UnityEvent onUnfreeze = null;
    public UnityEvent<bool> onIsFrozen = null;

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

    public void SetPaused(bool value, bool overrideCursor = false, bool showCursor = true)
    {
        if (pausingOrUnpausing || !canPause)
            return;

        // Don't pause if there's no player in the scene
        if (value && Player.GetPlayer() == null)
            return;

        pausingOrUnpausing = true;

        SetTimeFreeze(value);
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

        if (overrideCursor)
        {
            GameManager.SetCursorEnabled(showCursor);
        }
        else
        {
            if (Player.GetPlayer() == null)
                GameManager.SetCursorEnabled(true);
            else
                GameManager.SetCursorEnabled(value);
        }

        pausingOrUnpausing = false;
    }

    public void SetPaused(bool value) { SetPaused(value, false, true); }

    public void Pause() { SetPaused(true); }
    public void Pause(bool showCursor) { SetPaused(true, true, showCursor); }

    public void Unpause() { SetPaused(false); }
    public void Unpause(bool showCursor) { SetPaused(false, true, showCursor); }

    public void SetTimeFreeze(bool value)
    {
        Time.timeScale = value ? 0.0f : 1.0f;

        /*
        // Handle freeze feedback
        if (freezeFeedback != null)
        {
            if (value)
            {
                freezeFeedback.PlayFeedbacks();
            }
            else
            {
                freezeFeedback.StopFeedbacks();
            }
        }
        else
        {
            Debug.LogError("PauseHandler: The Freeze Feedbacks has not been set and is null! You probably need to assign it in the inspector.", this);
        }
        */

        // Disables player controls if they were enabled and we're freezing
        if (InputManager.inputMaster.Player.enabled && value)
        {
            InputManager.SetActionMapActive(InputManager.inputMaster.Player, false);
            disabledControls = true;
        }
        // Re-enables player controls if we disabled them and we're unfreezing
        else if (disabledControls && !value)
        {
            InputManager.SetActionMapActive(InputManager.inputMaster.Player, true);
            disabledControls = false;
        }

        // Handle events
        onIsFrozen.Invoke(value);
        if (value)
            onFreeze.Invoke();
        else
            onUnfreeze.Invoke();
    }
}