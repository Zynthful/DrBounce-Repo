using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Automatically starts and stops listening via OnEnable and OnDisable.")]
    private bool autoListenOnEnable = true;

    [SerializeField]
    private UnityEvent onActionPerformed = null;

    [SerializeField]
    private InputActionReference inputAction;

    [SerializeField]
    private bool delayUseScaledTime = false;

    [SerializeField]
    [Tooltip("Delay before we start listening for input. Only works with OnEnable right now.")]
    private float listenDelay = 0.0f;

    private void OnEnable()
    {
        if (autoListenOnEnable)
            StartCoroutine(DelayListen());
    }

    private void OnDisable()
    {
        if (autoListenOnEnable)
            StopListening();
    }

    public void Listen()
    {
        inputAction.action.performed += OnActionPerformed;
        inputAction.action.Enable();
    }

    public void StopListening()
    {
        inputAction.action.performed -= OnActionPerformed;
        inputAction.action.Disable();
    }

    private void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        onActionPerformed.Invoke();
    }

    private IEnumerator DelayListen()
    {
        if (delayUseScaledTime)
            yield return new WaitForSeconds(listenDelay);
        else
            yield return new WaitForSecondsRealtime(listenDelay);

        Listen();
    }
}