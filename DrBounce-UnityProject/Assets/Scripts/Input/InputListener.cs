using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onActionPerformed = null;

    [SerializeField]
    private InputActionReference inputAction;

    private void OnEnable()
    {
        inputAction.action.performed += OnActionPerformed;
        inputAction.action.Enable();
    }

    private void OnDisable()
    {
        inputAction.action.performed -= OnActionPerformed;
        inputAction.action.Disable();
    }

    private void OnActionPerformed(InputAction.CallbackContext ctx)
    {
        onActionPerformed.Invoke();
    }
}