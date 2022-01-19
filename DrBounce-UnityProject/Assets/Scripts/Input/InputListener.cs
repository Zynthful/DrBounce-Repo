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
        inputAction.action.performed += _ => onActionPerformed.Invoke();
        inputAction.action.Enable();
    }

    private void OnDisable()
    {
        inputAction.action.performed -= _ => onActionPerformed.Invoke();
        inputAction.action.Disable();
    }
}
