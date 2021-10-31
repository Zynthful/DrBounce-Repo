using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownSetting : Settings
{
    [SerializeField]
    private TMP_Dropdown dropdown = null;

    // This is an int because Unity's dropdowns pass ints through OnValueChanged()
    [SerializeField]
    private GameEventInt onValueChanged = null;

    protected virtual void Start()
    {
        int initialValue = PlayerPrefs.GetInt($"Options/{type}/{settingName}", dropdown.value);
        onValueChanged?.Raise(initialValue);
        dropdown.value = initialValue;
    }

    protected virtual void OnEnable()
    {
        dropdown.onValueChanged.AddListener(SetValue);
    }

    protected virtual void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(SetValue);
    }

    public void SetValue(int value)
    {
        onValueChanged?.Raise(value);
        PlayerPrefs.SetInt($"Options/{type}/{settingName}", value);

        Save();
    }
}
