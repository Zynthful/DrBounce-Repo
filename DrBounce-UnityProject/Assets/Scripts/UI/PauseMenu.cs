using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    private bool paused = false;

    public InputMaster controls;

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

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onPause = null;
    [SerializeField]
    private UnityEvent _onUnpause = null;
    [SerializeField]
    private BoolEvent _onPauseStateChange = null;

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void Start()
    {
        if (unpauseOnStart)
            SetPaused(false);
    }

    private void InvertPause()
    {
        SetPaused(!paused);
    }

    public void SetPaused(bool value)
    {
        paused = value;

        // Update cursor lock state and visibility
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;

        // Raise events
        onPauseStateChange?.Raise(paused);
        _onPauseStateChange.Invoke(paused);
        if (paused)
        {
            onPause?.Raise();
            _onPause.Invoke();
        }
        else
        {
            onUnpause?.Raise();
            _onUnpause.Invoke();
        }
    }

    private void OnEnable()
    {
        controls.Menu.Pause.performed += _ => InvertPause();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Menu.Pause.performed -= _ => InvertPause();
        controls.Disable();
    }
}
