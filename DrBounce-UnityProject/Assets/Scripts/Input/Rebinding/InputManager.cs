using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputMaster inputMaster = null;

    // Events
    public static event Action<InputAction, int> onRebindBegin = null;
    public static event Action onRebindComplete = null;
    public static event Action onRebindCancel = null;
    public static event Action onRebindReset = null;

    private void Awake()
    {
        CheckForNullInputMaster();
    }

    /// <summary>
    /// Attempts to begin rebinding the given action name.
    /// </summary>
    /// <param name="actionName">The name of the action to be rebinded.</param>
    /// <param name="index">The action binding index, i.e., the control group, e.g., Mouse and Keyboard.</param>
    /// <param name="statusText">The text to be updated when rebinding.</param>
    /// <param name="controlExclusions">Optional. Array of control paths that are excluded from the binding. These controls can't be used for the binding.</param>
    public static void BeginRebind(string actionName, int index, TextMeshProUGUI statusText, string[] controlExclusions = null, string[] cancelButtons = null)
    {
        InputAction action = inputMaster.asset.FindAction(actionName);

        // Check our action and index is valid
        if (action != null && action.bindings.Count > index && index >= 0)
        {
            if (action.bindings[index].isComposite)
            {
                var firstPartIndex = index + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                {
                    Rebind(action, index, true, statusText, controlExclusions, cancelButtons);
                }
            }
            else
            {
                Rebind(action, index, false, statusText, controlExclusions, cancelButtons);
            }
        }
    }

    /// <summary>
    /// Performs an interactive rebinding of the given action.
    /// </summary>
    /// <param name="action">The action to be rebinded.</param>
    /// <param name="index">The action binding index, i.e., the control group, e.g., Mouse and Keyboard.</param>
    /// <param name="allCompositeParts">If all our bindings for the given action are composite.</param>
    /// <param name="statusText">The text to be updated when rebinding.</param>
    /// <param name="controlExclusions">Optional. Array of control paths that are excluded from the binding. These controls can't be used for the binding.</param>
    private static void Rebind(InputAction action, int index, bool allCompositeParts, TextMeshProUGUI statusText, string[] controlExclusions = null, string[] cancelButtons = null)
    {
        if (action == null || index < 0)
            return;

        statusText.text = $"Press a {action.expectedControlType}";

        action.Disable();

        // Create instance for rebinding
        var rebind = action.PerformInteractiveRebinding(index);

        // Cancel rebinding with given button paths
        for (int i = 0; i < cancelButtons.Length; i++)
        {
            rebind.WithCancelingThrough(cancelButtons[i]);
        }

        // Exclude any given controls to exclude
        for (int i = 0; i < controlExclusions.Length; i++)
        {
            rebind.WithControlsExcluding(controlExclusions[i]);
        }

        // Rebind completed operation
        rebind.OnComplete(operation =>
        {
            action.Enable();
            operation.Dispose();

            // Rebind next binding if it is composite
            if (allCompositeParts)
            {
                var nextIndex = index + 1;
                if (nextIndex < action.bindings.Count && action.bindings[nextIndex].isComposite)
                {
                    Rebind(action, nextIndex, allCompositeParts, statusText, controlExclusions);
                }
            }

            SaveBindingOverride(action);
            onRebindComplete?.Invoke();
        });

        // Rebind cancelled operation
        rebind.OnCancel(operation =>
        {
            action.Enable();
            operation.Dispose();

            onRebindCancel?.Invoke();
        });

        onRebindBegin?.Invoke(action, index);
        rebind.Start();
    }

    /// <summary>
    /// Reset the given action binding to its default.
    /// </summary>
    /// <param name="actionName">The name of the action to reset.</param>
    /// <param name="index">The action binding index, i.e., the control group, e.g., Mouse and Keyboard.</param>
    public static void ResetBinding(string actionName, int index)
    {
        InputAction action = inputMaster.asset.FindAction(actionName);

        if (action != null && action.bindings.Count > index)
        {
            // Check if the action we're trying to reset is composite
            if (action.bindings[index].isComposite)
            {
                // Remove all composite binding overrides for this action
                for (int i = index; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                {
                    action.RemoveBindingOverride(i);
                }
            }
            else
            {
                action.RemoveBindingOverride(index);
            }

            SaveBindingOverride(action);
        }
    }

    /// <summary>
    /// Returns the binding name of the given action.
    /// </summary>
    /// <param name="actionName">The name of the action to find and retrieve the binding name of.</param>
    /// <param name="index">The action binding index, i.e., the control group, e.g., Mouse and Keyboard.</param>
    /// <returns></returns>
    public static string GetBindingName(string actionName, int index)
    {
        CheckForNullInputMaster();
        InputAction action = inputMaster.asset.FindAction(actionName);
        return action.GetBindingDisplayString(index);
    }

    /// <summary>
    /// Checks if we don't have an InputMaster, and if so, creates one.
    /// </summary>
    private static void CheckForNullInputMaster()
    {
        if (inputMaster == null)
            inputMaster = new InputMaster();
    }

    #region Save / Load
    #region Save
    /// <summary>
    /// Save an action's bindings to PlayerPrefs.
    /// </summary>
    /// <param name="actionName">The action name to save.</param>
    private static void SaveBindingOverride(string actionName)
    {
        InputAction action = inputMaster.asset.FindAction(actionName);
        SaveBindingOverride(action);
    }

    /// <summary>
    /// Save an action's bindings to PlayerPrefs.
    /// </summary>
    /// <param name="action">The action to save.</param>
    private static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString($"{action.actionMap}: {action.name} - {i}", action.bindings[i].overridePath);
        }
    }
    #endregion
    #region Load
    /// <summary>
    /// Using the given action, loads any overriden bindings from PlayerPrefs.
    /// </summary>
    /// <param name="actionName">The name of the action to load overriding bindings from.</param>
    public static void LoadBindingOverride(string actionName)
    {
        CheckForNullInputMaster();
        InputAction action = inputMaster.asset.FindAction(actionName);
        LoadBindingOverride(action);
    }

    /// <summary>
    /// Using the given action, loads any overriden bindings from PlayerPrefs.
    /// </summary>
    /// <param name="action">The action to load overriding bindings from.</param>
    public static void LoadBindingOverride(InputAction action)
    {
        // Loop through action bindings and apply any existing overrides from PlayerPrefs
        for (int i = 0; i < action.bindings.Count; i++)
        {
            string path = PlayerPrefs.GetString($"{action.actionMap}: {action.name} - {i}");
            if (!string.IsNullOrEmpty(path))
            {
                action.ApplyBindingOverride(i, path);
            }
        }
    }
    #endregion
    #endregion
}
