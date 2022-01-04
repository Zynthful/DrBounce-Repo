using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RebindUI : MonoBehaviour
{
    [Header("Rebinding Settings")]

    [SerializeField]
    [Tooltip("The action to be rebinded.")]
    private InputActionReference inputActionReference = null;

    [SerializeField]
    [Tooltip("The binding index to rebind. This represents the control scheme, e.g., Mouse & Keyboard.")]
    [Range(0, 10)]
    private int selectedBinding = 0;

    [SerializeField]
    [Tooltip("Optional. Control paths that are excluded from the binding. These controls can't be used for the binding. These *must* match the exact path of the binding.")]
    private string[] controlsToExclude = null;

    [SerializeField]
    [Tooltip("Optional. Control paths that cancel the rebinding operation. These *must* match the exact path of the binding.")]
    private string[] rebindCancelButtons = null;

    //TODO
    //[SerializeField]
    //private InputBinding.DisplayStringOptions displayStringOptions = new InputBinding.DisplayStringOptions();

    private InputBinding inputBinding;      // Displays information about the selected action binding (path, group, etc.).
    private int bindingIndex;               // Actual binding index clamped between available bindings.
    private string actionName;              // Name of the action to be rebinded, taken from inputActionReference.

    [Header("UI")]
    [SerializeField]
    [Tooltip("Text displaying the name of the action to rebind.")]
    private TextMeshProUGUI actionText = null;
    [SerializeField]
    [Tooltip("Button which triggers rebinding.")]
    private Button rebindButton = null;
    [SerializeField]
    [Tooltip("Text displaying the action binding.")]
    private TextMeshProUGUI rebindText = null;
    [SerializeField]
    [Tooltip("Button which triggers resetting the action bindings to default.")]
    private Button resetButton = null;

    [Header("Events")]
    [SerializeField]
    private UnityEvent<InputAction, int> onRebindBegin = null;  // Invoked on pressing the rebind button
    [SerializeField]
    private UnityEvent onRebindEnd = null;                      // Invoked on both completion or cancelling of rebinding
    [SerializeField]
    private UnityEvent onRebindComplete = null;                 // Invoked on successful rebinding
    [SerializeField]
    private UnityEvent onRebindCancel = null;                   // Invoked on rebind cancelling by pressing the cancel button
    [SerializeField]
    private UnityEvent onRebindReset = null;                    // Invoked on pressing the reset button


    private void OnEnable()
    {
        // Listen to button onClick events
        rebindButton.onClick.AddListener(() => Rebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if (inputActionReference != null)
        {
            // Load binding overrides from PlayerPrefs, if they exist
            InputManager.LoadBindingOverride(actionName);

            UpdateBindingInfo();
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        rebindButton.onClick.RemoveListener(() => Rebind());
        resetButton.onClick.RemoveListener(() => ResetBinding());
    }

    private void OnValidate()
    {
        if (inputActionReference != null)
        {
            UpdateBindingInfo();
            UpdateUI();
        }
    }

    private void Rebind()
    {
        onRebindBegin?.Invoke(inputActionReference.action, bindingIndex);
        InputManager.onRebindComplete += Complete;
        InputManager.onRebindCancel += Cancel;
        InputManager.BeginRebind(actionName, bindingIndex, rebindText, controlsToExclude, rebindCancelButtons);
    }

    private void ResetBinding()
    {
        onRebindReset.Invoke();
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }

    private void Complete()
    {
        onRebindComplete?.Invoke();
        InputManager.onRebindComplete -= Complete;
        InputManager.onRebindCancel -= Cancel;
        UpdateUI();
    }

    private void Cancel()
    {
        onRebindCancel.Invoke();
        InputManager.onRebindComplete -= Complete;
        InputManager.onRebindCancel -= Cancel;
        UpdateUI();
    }

    /// <summary>
    /// Update action name, input binding and binding index.
    /// </summary>
    private void UpdateBindingInfo()
    {
        actionName = inputActionReference.action.name;

        if (inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    /// <summary>
    /// Update action and rebind text
    /// </summary>
    private void UpdateUI()
    {
        if (actionText != null)
        {
            actionText.text = actionName;
        }

        if (rebindText != null)
        {
            if (Application.isPlaying)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
            }
            else
            {
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
            }
        }
    }

    /// <summary>
    /// Returns the current InputBinding.
    /// </summary>
    /// <returns></returns>
    public InputBinding GetInputBinding()
    {
        return inputBinding;
    }
}