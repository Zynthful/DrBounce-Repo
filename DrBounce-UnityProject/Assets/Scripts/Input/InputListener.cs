using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onAction = null;

    [SerializeField]
    private InputMaster.MenuActions menuActions;

    [SerializeField]
    private InputAction inputAction;

    private void OnEnable()
    {
        inputAction.performed += _ => onAction.Invoke();
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.performed -= _ => onAction.Invoke();
        inputAction.Disable();
    }
}
