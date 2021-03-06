using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input Action Binding Setting", menuName = "ScriptableObjects/Settings/Input Action Binding")]
public class InputActionSetting : SettingData
{
    protected string currentValue = null;

    [Header("Input Action References")]
    [SerializeField]
    [Tooltip("The input action to rebind and store.")]
    protected InputActionReference actionReference = null;
    [SerializeField]
    [Tooltip("The input actions that the main input action be rebinded will be prevented from sharing the same binding.")]
    protected InputActionReference[] blockingActions = null;
    [SerializeField]
    [Tooltip("The binding index to rebind.")]
    [Range(0, 10)]
    private int selectedBinding = 0;

    private InputBinding inputBinding;      // Displays information about the selected action binding (path, group, etc.).
    private int bindingIndex;               // Actual binding index clamped between available bindings.
    private string actionName;              // Name of the action to be rebinded, taken from the input action reference.
    private string[] blockingActionNames = null;

    public override void Initialise()
    {
        base.Initialise();

        string path = PlayerPrefs.GetString($"Options/{type}/{subType}/{actionReference.action.actionMap}/{actionReference.action.name}/{bindingIndex}");
        if (!string.IsNullOrEmpty(path))
        {
            InputManager.OverrideBinding(actionName, path, bindingIndex);
        }

        UpdateBindingInfo();
    }

    private void OnValidate()
    {
        UpdateBindingInfo();
        UpdateBlockingActionsInfo();
    }

    public virtual void SetValue(string value)
    {
        currentValue = value;

        PlayerPrefs.SetString($"Options/{type}/{subType}/{actionReference.action.actionMap}/{actionReference.action.name}/{bindingIndex}", value);

        Save();
    }

    public string GetValue()
    {
        return currentValue;
    }

    public int GetBindingIndex()
    {
        return bindingIndex;
    }

    public bool IsSelectedBindingValid()
    {
        return actionReference.action.bindings.Count > selectedBinding && selectedBinding >= 0;
    }

    public InputAction GetAction()
    {
        return actionReference.action;
    }

    public string GetActionName()
    {
        if (string.IsNullOrEmpty(actionName))
        {
            UpdateBindingInfo();
        }
        return actionName;
    }

    /// <summary>
    /// Update action name, input binding and binding index.
    /// </summary>
    private void UpdateBindingInfo()
    {
        if (actionReference != null)
        {
            actionName = actionReference.action.name;
            if (IsSelectedBindingValid())
            {
                inputBinding = actionReference.action.bindings[selectedBinding];
                bindingIndex = selectedBinding;
            }
        }
        else
        {
            actionName = null;
            inputBinding = new InputBinding();
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateBlockingActionsInfo()
    {
        if (blockingActions == null)
            return;

        int validActions = 0;
        for (int i = 0; i < blockingActions.Length; i++)
        {
            if (blockingActions[i] == actionReference)
            {
                blockingActions[i] = null;
            }

            if (blockingActions[i] != null)
            {
                validActions++;
            }
        }

        blockingActionNames = new string[validActions];
        for (int i = 0; i < validActions; i++)
        {
            blockingActionNames[i] = blockingActions[i].action.name;
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

    public InputActionReference[] GetBlockingActions()
    {
        return blockingActions;
    }

    public string[] GetBlockingActionNames()
    {
        return blockingActionNames;
    }

    public string GetEffectivePath()
    {
        return actionReference.action.bindings[bindingIndex].effectivePath;
    }
}
