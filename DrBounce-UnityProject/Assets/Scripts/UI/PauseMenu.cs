using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;
    private bool hasPausedOnce = false; // Has the player paused at least once since initialisation?

    [Header("Pause Settings")]
    [SerializeField]
    private bool unpauseOnStart = true;

    [Header("Game Event Declarations")]
    [SerializeField]
    private GameEventBool onPauseStateChange = null;
    [SerializeField]
    private GameEvent onPause = null;
    [SerializeField]
    private GameEvent onUnpause = null;
    [SerializeField]
    private GameEvent onUnpauseAfterPause = null;

    [Header("Unity Events")]
    public UnityEvent _onPause = null;
    public UnityEvent _onUnpause = null;
    public UnityEvent _onUnpauseAfterPause = null;
    public BoolEvent _onPauseStateChange = null;

    private void Start()
    {
        if (unpauseOnStart)
            SetPaused(false);
    }

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
        SetPaused(!paused);
    }

    public void SetPaused(bool value)
    {
        paused = value;
        
        // Enable/disable gameplay controls
        InputManager.SetActionMapActive(InputManager.inputMaster.Player, !value);

        // Update cursor lock state and visibility
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;

        // Raise events
        onPauseStateChange?.Raise(paused);
        _onPauseStateChange.Invoke(paused);
        if (paused)
        {
            hasPausedOnce = true;
            onPause?.Raise();
            _onPause.Invoke();
        }
        else
        {
            onUnpause?.Raise();
            _onUnpause.Invoke();

            if (hasPausedOnce)
            {
                onUnpauseAfterPause?.Raise();
                _onUnpauseAfterPause.Invoke();
            }
        }
    }
}
