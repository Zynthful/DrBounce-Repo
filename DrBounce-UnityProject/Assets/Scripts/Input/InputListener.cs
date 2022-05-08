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

    private void OnEnable()
    {
        if (autoListenOnEnable)
            Listen();
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
}