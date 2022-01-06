using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RebindUI : Settings
{
    [Header("Rebinding Settings")]
    [SerializeField]
    [Tooltip("Setting scriptable object which stores info about the input action.")]
    protected InputActionSetting setting = null;

    [SerializeField]
    [Tooltip("Optional. Control paths that are excluded from the binding. These controls can't be used for the binding. These *must* match the exact path of the binding.")]
    private string[] controlsToExclude = null;

    [SerializeField]
    [Tooltip("Optional. Control paths that cancel the rebinding operation. These *must* match the exact path of the binding.")]
    private string[] rebindCancelButtons = null;

    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions = new InputBinding.DisplayStringOptions();

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


    protected override void OnEnable()
    {
        base.OnEnable();

        // Listen to button onClick events
        rebindButton.onClick.AddListener(() => Rebind());
        resetButton.onClick.AddListener(() => ResetToDefault());

        setting.OnResetDefault += ResetToDefault;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        rebindButton.onClick.RemoveListener(() => Rebind());
        resetButton.onClick.RemoveListener(() => ResetToDefault());

        setting.OnResetDefault -= ResetToDefault;
    }

    private void OnValidate()
    {
        UpdateUI();
    }

    private void Rebind()
    {
        onRebindBegin?.Invoke(setting.GetAction(), setting.GetBindingIndex());
        InputManager.onRebindComplete += Complete;
        InputManager.onRebindCancel += Cancel;
        InputManager.BeginRebind(setting.GetActionName(), setting.GetBindingIndex(), rebindText, setting, controlsToExclude, rebindCancelButtons, setting.GetBlockingActionNames());
    }

    protected override void ResetToDefault()
    {
        onRebindReset.Invoke();
        InputManager.ResetBinding(setting.GetActionName(), setting.GetBindingIndex(), setting);
        base.ResetToDefault();
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
    /// Update action and rebind text
    /// </summary>
    protected override void UpdateUI()
    {
        base.UpdateUI();
        if (setting != null && setting.GetAction() != null)
        {
            if (actionText != null)
            {
                actionText.text = setting.GetActionName();
            }

            if (rebindText != null)
            {
                rebindText.text = InputManager.GetBindingDisplayString(setting.GetActionName(), setting.GetBindingIndex(), displayStringOptions);
            }
        }
        else
        {
            rebindText.text = "No Action Selected";
            actionText.text = "No Action Selected";
        }
    }
}