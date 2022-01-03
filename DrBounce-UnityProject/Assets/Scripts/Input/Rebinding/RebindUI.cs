using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class RebindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference inputActionReference = null;

    [SerializeField]
    private bool excludeMouse = true;

    [SerializeField]
    [Range(0, 10)]
    private int selectedBinding = 0;

    [SerializeField]
    private string[] controlsToExclude = new string[0];

    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions = new InputBinding.DisplayStringOptions();

    private InputBinding inputBinding;

    private int bindingIndex;
    private string actionName;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI actionText = null;
    [SerializeField]
    private Button rebindButton = null;
    [SerializeField]
    private TextMeshProUGUI rebindText = null;
    [SerializeField]
    private Button resetButton = null;

    [Header("Events")]
    [SerializeField]
    private UnityEvent<InputAction, int> onRebindBegin = null;
    [SerializeField]
    private UnityEvent onRebindComplete = null;
    [SerializeField]
    private UnityEvent onRebindCancel = null;


    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => Rebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        InputManager.onRebindBegin += onRebindBegin.Invoke;
        InputManager.onRebindComplete += Complete;
        InputManager.onRebindCancel += onRebindCancel.Invoke;

        if (inputActionReference != null)
        {
            InputManager.LoadBindingOverride(actionName);
            CheckBinding();
        }
    }

    private void OnDisable()
    {
        rebindButton.onClick.RemoveListener(() => Rebind());
        resetButton.onClick.RemoveListener(() => ResetBinding());
    }

    private void OnValidate()
    {
        CheckBinding();
    }

    private void CheckBinding()
    {
        if (inputActionReference != null)
        {
            GetBindingInfo();
            UpdateUI();
        }
    }

    private void Rebind()
    {
        InputManager.BeginRebind(actionName, bindingIndex, rebindText, controlsToExclude);
    }

    private void ResetBinding()
    {
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }

    private void Complete()
    {
        onRebindComplete?.Invoke();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if (inputActionReference.action != null)
        {
            actionName = inputActionReference.action.name;

            if (inputActionReference.action.bindings.Count > selectedBinding)
            {
                inputBinding = inputActionReference.action.bindings[selectedBinding];
                bindingIndex = selectedBinding;
            }
        }
    }

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

    public string GetInputPath()
    {
        return inputBinding.path;
    }

    public string GetInputGroup()
    {
        return inputBinding.groups;
    }
}