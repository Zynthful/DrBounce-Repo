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
    [Tooltip("The binding index to rebind.")]
    [Range(0, 10)]
    private int selectedBinding = 0;

    private InputBinding inputBinding;      // Displays information about the selected action binding (path, group, etc.).
    private int bindingIndex;               // Actual binding index clamped between available bindings.
    private string actionName;              // Name of the action to be rebinded, taken from the input action reference.

    public override void Initialise()
    {
        base.Initialise();

        actionReference.action.RemoveBindingOverride(bindingIndex);

        string path = PlayerPrefs.GetString($"Options/{type}/{subType}/{actionReference.action.actionMap}/{actionReference.action.name}/{bindingIndex}");
        if (!string.IsNullOrEmpty(path))
        {
            //Debug.Log($"we got one bois: path: {path}, action: {actionReference.action.name}, binding: {bindingIndex}");
            actionReference.action.ApplyBindingOverride(bindingIndex, path);
        }

        /*
        currentValues.Clear();

        List<string> paths = new List<string>();
        for (int i = 0; i < actionReference.action.bindings.Count; i++)
        {
            string path = PlayerPrefs.GetString($"Options/{type}/{subType}/{actionReference.action.actionMap}/{actionReference.action.name}/{i}");
            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log($"we got one bois: path: {path}, action: {actionReference.action.name}, binding: {i}");

                actionReference.action.ApplyBindingOverride(i, path);
                paths.Add(path);
            }
        }
        for (int i = 0; i < paths.Count; i++)
        {
            AddValue(paths[i]);
        }
        */
    }

    private void OnValidate()
    {
        UpdateBindingInfo();
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

    /*
    public virtual void AddValue(string value)
    {
        currentValues.Add(value);
        Debug.Log($"Adding: {actionReference.action.name}/Binding/{currentValues.Count - 1} - there are now {currentValues.Count} bindings registered.");
        PlayerPrefs.SetString($"Options/{type}/{subType}/{actionReference.action.actionMap}/{actionReference.action.name}/Binding/{currentValues.Count - 1}", value);
    }

    public virtual bool Contains(string value)
    {
        return currentValues.Contains(value);
    }

    public int IndexOf(string value)
    {
        return currentValues.IndexOf(value);
    }

    public virtual void SetValue(string value, int index)
    {
        currentValues[index] = value;

        PlayerPrefs.SetString($"Options/{type}/{subType}/{actionReference.action.actionMap}/{actionReference.action.name}/{index}", value);

        Save();
    }

    public string GetValue(int index)
    {
        if (index < currentValues.Count)
            return currentValues[index];
        else 
            return null;
    }
    */

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

    /// <summary>
    /// Returns the current InputBinding.
    /// </summary>
    /// <returns></returns>
    public InputBinding GetInputBinding()
    {
        return inputBinding;
    }
}
