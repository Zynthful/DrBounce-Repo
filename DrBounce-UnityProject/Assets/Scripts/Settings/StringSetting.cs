using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New String Setting", menuName = "ScriptableObjects/Settings/String")]
public class StringSetting : SettingData
{
    protected string currentValue = null;

    [Header("Events")]
    [SerializeField]
    protected GameEventString onValueChanged = null;
    [SerializeField]
    protected GameEventString onResetToDefault = null;

    [Header("Value Settings")]
    [SerializeField]
    protected string defaultValue = null;

    public override void Initialise()
    {
        base.Initialise();
        currentValue = PlayerPrefs.GetString($"Options/{type}/{subType}/{settingName}", defaultValue);
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(string value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetString($"Options/{type}/{subType}/{settingName}", value);

        Save();
    }

    public override void ResetToDefault()
    {
        SetValue(defaultValue);
        onResetToDefault?.Raise(defaultValue);
        base.ResetToDefault();
    }

    public virtual string GetValue()
    {
        return currentValue;
    }
}
