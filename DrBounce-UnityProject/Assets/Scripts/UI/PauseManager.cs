using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class PauseHandler
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

    public delegate void BoolEvent(bool frozen);
    public static event BoolEvent onIsFrozen;
    public static event BoolEvent onIsPaused;

    public delegate void Event();
    public static event Event onUnpauseAfterPause;

    public static void InvertPause()
    {
        SetPaused(!GameManager.s_Instance.paused);
    }

    public static void SetPaused(bool value, bool overrideCursor = false, bool showCursor = true)
    {
        if (pausingOrUnpausing || !canPause)
            return;

        // Don't pause if there's no player in the scene
        if (value && Player.GetPlayer() == null)
            return;

        pausingOrUnpausing = true;

        SetTimeFreeze(value);
        onIsPaused.Invoke(value);

        if (value)
        {
            hasPausedOnce = true;
        }
        else
        {
            if (hasPausedOnce)
            {
                onUnpauseAfterPause.Invoke();
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

    public static void SetPaused(bool value) { SetPaused(value, false, true); }

    public static void Pause() { SetPaused(true); }
    public static void Pause(bool showCursor) { SetPaused(true, true, showCursor); }

    public static void Unpause() { SetPaused(false); }
    public static void Unpause(bool showCursor) { SetPaused(false, true, showCursor); }

    public static void SetTimeFreeze(bool value)
    {
        Time.timeScale = value ? 0.0f : 1.0f;
        GameManager.s_Instance.paused = value;
        onIsFrozen.Invoke(value);

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
    }

    public static IEnumerator FreezeFrame(float duration)
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
    }
}

public class PauseManager : MonoBehaviour
{
    [Header("Pause Events")]
    public UnityEvent<bool> onIsPaused = null;
    public UnityEvent onPause;
    public UnityEvent onUnpause;
    public UnityEvent onUnpauseAfterPause;

    [Header("Freeze Events")]
    public UnityEvent<bool> onIsFrozen;
    public UnityEvent onFreeze;
    public UnityEvent onUnfreeze;

    private void OnEnable()
    {
        InputManager.inputMaster.Menu.Pause.performed += _ => PauseHandler.InvertPause();
        PauseHandler.onIsFrozen += InvokeFreezeEvents;
        PauseHandler.onIsPaused += InvokePauseEvents;
        PauseHandler.onUnpauseAfterPause += InvokeUnpauseAfterPauseEvent;
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Menu.Pause.performed -= _ => PauseHandler.InvertPause();
        PauseHandler.onIsFrozen -= InvokeFreezeEvents;
    }

    private void InvokeUnpauseAfterPauseEvent()
    {
        onUnpauseAfterPause.Invoke();
    }

    private void InvokeFreezeEvents(bool value)
    {
        onIsFrozen.Invoke(value);

        if (value)
            onFreeze.Invoke();
        else
            onUnfreeze.Invoke();
    }

    private void InvokePauseEvents(bool value)
    {
        onIsPaused.Invoke(value);

        if (value)
            onPause.Invoke();
        else
            onUnpause.Invoke();
    }

    public void FreezeForDuration(float duration)
    {
        if (Time.timeScale != 0)
        {
            StartCoroutine(PauseHandler.FreezeFrame(duration));
        }
    }

    public void SetPaused(bool value)
    {
        PauseHandler.SetPaused(value);
    }

    public void SetTimeFreeze(bool value)
    {
        PauseHandler.SetTimeFreeze(value);
    }
}