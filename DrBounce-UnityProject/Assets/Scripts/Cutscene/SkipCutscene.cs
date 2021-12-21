using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkipCutscene : MonoBehaviour
{
    private InputMaster controls = null;

    [Header("Input Settings")]
    [SerializeField]
    private float holdDuration = 1.0f;

    private bool holding = false;

    private float currentTime = 0.0f;

    [Header("Fade Settings")]
    [SerializeField]
    private float timeToTriggerFadeOut = 3.0f;

    private bool active = true;

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

    private void Awake()
    {
        controls = new InputMaster();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Cutscene.SkipCutscene.started += _ => StartSkip();
        controls.Cutscene.SkipCutscene.canceled += _ => ResetProgress();
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Cutscene.SkipCutscene.started -= _ => StartSkip();
        controls.Cutscene.SkipCutscene.canceled -= _ => ResetProgress();
    }

    private void Start()
    {
        StartCoroutine(TimeToFade());
    }

    private void Update()
    {
        if (holding)
        {
            currentTime += Time.deltaTime;

            onProgressChange.Invoke(currentTime / holdDuration);

            if (currentTime >= holdDuration)
            {
                ResetProgress();
                Finish();
            }
        }
    }

    private void ResetProgress()
    {
        holding = false;
        currentTime = 0;
        onProgressChange.Invoke(0);
        StopAllCoroutines();
        StartCoroutine(TimeToFade());
    }

    private void Finish()
    {
        onFinish.Invoke();
        _onFinish?.Raise();
    }

    private IEnumerator TimeToFade()
    {
        yield return new WaitForSeconds(timeToTriggerFadeOut);
        active = false;
        onBeginFadeOut.Invoke();
    }

    private void StartSkip()
    {
        holding = true;
        if (!active)
        {
            onStartSkip.Invoke();
            active = true;
        }
    }
}