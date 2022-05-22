using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour
{
    public delegate void CameraAnim();
    public static event CameraAnim OnEnd;

    [SerializeField]
    private Image buttonPrompt = null;

    [Header("Input Settings")]
    [SerializeField]
    private float holdDuration = 1.0f;

    private bool holding = false;

    private float currentTime = 0.0f;

    [Header("Fade Settings")]
    [SerializeField]
    private float timeToTriggerFadeOut = 3.0f;

    private bool promptActive = true;
    private bool skipped = false;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onStartSkip = null;
    [SerializeField]
    private UnityEvent onBeginFadeOut = null;
    [SerializeField]
    private UnityEvent<float> onProgressChange = null;      // Passes current progress between 0 and 1
    [SerializeField]
    private UnityEvent onFinish = null;
    [SerializeField]
    private GameEvent _onFinish = null;

    private void OnEnable()
    {
        InputManager.inputMaster.Menu.Continue.started += OnSkipStarted;
        InputManager.inputMaster.Menu.Continue.canceled += OnSkipCancelled;
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Menu.Continue.started -= OnSkipStarted;
        InputManager.inputMaster.Menu.Continue.canceled -= OnSkipCancelled;
    }

    private void Start()
    {
        StartCoroutine(FadeDelay());
    }

    private void Update()
    {
        if (holding)
        {
            currentTime += Time.deltaTime;

            float progress = currentTime / holdDuration;
            buttonPrompt.fillAmount = progress;
            onProgressChange.Invoke(progress);

            if (currentTime >= holdDuration)
            {
                ResetProgress(false);
                Finish();
            }
        }
    }

    private void OnSkipStarted(InputAction.CallbackContext ctx)
    {
        StartSkip();
    }

    private void OnSkipCancelled(InputAction.CallbackContext ctx)
    {
        ResetProgress(true);
    }

    private void ResetProgress(bool doFadeDelay)
    {
        holding = false;
        buttonPrompt.fillAmount = 1.0f;
        currentTime = 0;
        onProgressChange.Invoke(0);
        if (this != null) StopAllCoroutines();
        if (!skipped && doFadeDelay && gameObject.activeInHierarchy)
        {
            StartCoroutine(FadeDelay());
        }
    }

    private void Finish()
    {
        skipped = true;
        onFinish.Invoke();
        _onFinish?.Raise();
        OnEnd?.Invoke();
    }

    private IEnumerator FadeDelay()
    {
        yield return new WaitForSeconds(timeToTriggerFadeOut);
        promptActive = false;
        onBeginFadeOut.Invoke();
    }

    private void StartSkip()
    {
        holding = true;
        if (!promptActive)
        {
            onStartSkip.Invoke();
            promptActive = true;
        }
    }
}