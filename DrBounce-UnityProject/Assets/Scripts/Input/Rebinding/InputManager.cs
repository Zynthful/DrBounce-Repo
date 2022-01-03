using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputMaster inputMaster;

    private void Awake()
    {
        if (inputMaster == null)
        {
            inputMaster = new InputMaster();
        }
    }

    public static event Action<InputAction, int> onRebindBegin = null;
    public static event Action onRebindComplete = null;
    public static event Action onRebindCancel = null;

    public static void BeginRebind(string name, int index, TextMeshProUGUI statusText, string[] controlExclusions = null)
    {
        InputAction action = inputMaster.asset.FindAction(name);

        // Check our action and index is valid
        if (action != null && action.bindings.Count > index && index >= 0)
        {
            if (action.bindings[index].isComposite)
            {
                var firstPartIndex = index + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                {
                    Rebind(action, index, true, statusText, controlExclusions);
                }
            }
            else
            {
                Rebind(action, index, false, statusText, controlExclusions);
            }
        }
    }

    private static void Rebind(InputAction action, int index, bool allCompositeParts, TextMeshProUGUI statusText, string[] controlExclusions = null)
    {
        if (action == null || index < 0)
            return;

        statusText.text = $"Press a {action.expectedControlType}";

        action.Disable();

        // Create instance for rebinding
        var rebind = action.PerformInteractiveRebinding(index);

        rebind.OnComplete(operation =>
        {
            action.Enable();
            operation.Dispose();

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

        rebind.OnCancel(operation =>
        {
            action.Enable();
            operation.Dispose();

            onRebindCancel?.Invoke();
        });

        // Cancel rebinding with Escape or Start
        rebind.WithCancelingThrough("<Keyboard>/escape");
        rebind.WithCancelingThrough("<Gamepad>/start");

        // Exclude any given controls to exclude
        for (int i = 0; i < controlExclusions.Length; i++)
        {
            rebind.WithControlsExcluding(controlExclusions[i]);
        }

        onRebindBegin?.Invoke(action, index);
        rebind.Start(); // begin rebinding process
    }

    public static string GetBindingName(string actionName, int index)
    {
        if (inputMaster == null)
        {
            inputMaster = new InputMaster();
        }

        InputAction action = inputMaster.asset.FindAction(actionName);
        return action.GetBindingDisplayString(index);
    }

    private static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString($"{action.actionMap}: {action.name} - {i}", action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (inputMaster != null)
        {
            inputMaster = new InputMaster();
        }
        InputAction action = inputMaster.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            string path = PlayerPrefs.GetString($"{action.actionMap}: {action.name} - {i}");
            if (!string.IsNullOrEmpty(path))
            {
                action.ApplyBindingOverride(i, path);
            }
        }
    }

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
}
