using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputMaster inputMaster = null;

    public static InputManager s_Instance = null;

    // Events
    public static event Action<InputAction, int> onRebindBegin = null;
    public static event Action onRebindComplete = null;
    public static event Action onRebindCancel = null;
    public static event Action onRebindReset = null;
    public static event Action<InputAction, InputAction> onRebindMatchedBlockingAction = null;

    public InputDescription[] inputs = null;    // to associate sprites with inputs

    [Serializable]
    public struct InputDescription
    {
        public string path;
        public Sprite defaultSprite;
        public Sprite dualshockSprite;
    }

    public InputDescription FindDescription(string path)
    {
        return inputs.FirstOrDefault(InputDescription => InputDescription.path == path);
    }

    /*
    public Sprite GetSpriteFromControlPath(string path, InputDevice device)
    {
        return Resources.Load<Sprite>($"Sprites/Controller Icons/{device}/{path}");
    }
    */

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(InputManager)) as InputManager;
        }

        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }

        CheckForNullInputMaster();

        inputMaster.Disable();
        // Enables all controls
        // Any controls that should start disabled should be done AFTER this
        inputMaster.Enable();
    }

    /// <summary>
    /// Attempts to begin rebinding the given action name.
    /// </summary>
    /// <param name="actionName">The name of the action to be rebinded.</param>
    /// <param name="index">The action binding index, i.e., the control group, e.g., Mouse and Keyboard.</param>
    /// <param name="statusText">The text to be updated when rebinding.</param>
    /// <param name="controlExclusions">Optional. Array of control paths that are excluded from the binding. These controls can't be used for the binding.</param>
    public static void BeginRebind(string actionName, int index, TextMeshProUGUI statusText, InputActionSetting setting, string[] controlExclusions = null, string[] cancelButtons = null, string[] blockingActionNames = null)
    {
        InputAction action = inputMaster.asset.FindAction(actionName);

        InputAction[] blockingActions;
        if (blockingActionNames != null)
        {
            blockingActions = new InputAction[blockingActionNames.Length];
            for (int i = 0; i < blockingActions.Length; i++)
            {
                blockingActions[i] = inputMaster.asset.FindAction(blockingActionNames[i]);
            }
        }
        else
        {
            blockingActions = null;
        }

        // Check our action and index is valid
        if (action != null && action.bindings.Count > index && index >= 0)
        {
            if (action.bindings[index].isComposite)
            {
                var firstPartIndex = index + 1;
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                {
                    Rebind(action, index, true, statusText, setting, controlExclusions, cancelButtons, blockingActions);
                }
            }
            else
            {
                Rebind(action, index, false, statusText, setting, controlExclusions, cancelButtons, blockingActions);
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
    private static void Rebind(InputAction action, int index, bool allCompositeParts, TextMeshProUGUI statusText, InputActionSetting setting, string[] controlExclusions = null, string[] cancelButtons = null, InputAction[] blockingActions = null)
    {
        if (action == null || index < 0)
            return;

        statusText.text = $"Press a {action.expectedControlType}";

        // Store wasEnabled so we can re-enable the action if we disabled it
        bool wasEnabled = action.enabled;
        if (wasEnabled)
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
            // Re-enable our action if it was disabled previously
            if (wasEnabled)
                action.Enable();

            operation.Dispose();

            // Rebind next binding if it is composite
            if (allCompositeParts)
            {
                var nextIndex = index + 1;
                if (nextIndex < action.bindings.Count && action.bindings[nextIndex].isComposite)
                {
                    Rebind(action, nextIndex, allCompositeParts, statusText, setting, controlExclusions, cancelButtons, blockingActions);
                }
            }

            SaveBindingOverride(action, setting);
            onRebindComplete?.Invoke();
        });

        // On any control pressed during operation (i think..???)
        rebind.OnPotentialMatch(operation =>
        {
            if (blockingActions != null)
            {
                for (int i = 0; i < operation.candidates.Count; i++)
                {
                    for (int j = 0; j < blockingActions.Length; j++)
                    {
                        for (int k = 0; k < blockingActions[j].controls.Count; k++)
                        {
                            if (blockingActions[j].controls[k] == operation.candidates[i])
                            {
                                //Debug.Log("DING DING DING WE GOT A WINNER: " + operation.candidates[i].path);
                                //Debug.Log($"This {action.expectedControlType} is already mapped to {blockingActions[j].name}.");
                                onRebindMatchedBlockingAction?.Invoke(action, blockingActions[j]);

                                rebind.Cancel();

                                // todo:
                                // ui prompt and listen for Override and Cancel buttons.
                            }
                        }
                    }
                }
            }
        });

        // Rebind cancelled operation
        rebind.OnCancel(operation =>
        {
            // Re-enable our action if it was disabled previously
            if (wasEnabled)
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
    public static void ResetBinding(string actionName, int index, InputActionSetting setting)
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

            SaveBindingOverride(action, setting);
        }
    }

    public static string GetBindingDisplayString(string actionName, int bindingIndex, InputBinding.DisplayStringOptions displayOptions = new InputBinding.DisplayStringOptions())
    {
        CheckForNullInputMaster();
        return inputMaster.asset.FindAction(actionName).GetBindingDisplayString(bindingIndex, displayOptions);
    }

    /// <summary>
    /// Save a specified action setting's overriden binding
    /// </summary>
    /// <param name="action">The action to save.</param>
    private static void SaveBindingOverride(InputAction action, InputActionSetting setting)
    {
        setting.SetValue(action.bindings[setting.GetBindingIndex()].overridePath);
    }

    public static void OverrideBinding(string actionName, string path, int index)
    {
        CheckForNullInputMaster();
        inputMaster.asset.FindAction(actionName).ApplyBindingOverride(index, path);
    }

    public static void SetActionMapActive(InputActionMap map, bool active)
    {
        if (active)
        {
            //Debug.Log($"{map.name} controls enabled.");
            map.Enable();
        }
        else
        {
            //Debug.Log($"{map.name} controls disabled.");
            map.Disable();
        }
    }

    /// <summary>
    /// Checks if we don't have an InputMaster, and if so, creates one.
    /// </summary>
    public static void CheckForNullInputMaster()
    {
        if (inputMaster == null)
            inputMaster = new InputMaster();
    }
}
