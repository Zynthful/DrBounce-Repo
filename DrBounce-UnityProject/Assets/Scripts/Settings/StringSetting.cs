using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New String Setting", menuName = "ScriptableObjects/Settings/String")]
public class StringSetting : SettingData
{
    [SerializeField]
    protected GameEventString onValueChanged = null;

    [SerializeField]
    protected string defaultValue = null;

    protected string currentValue = null;

    public override void Initialise()
    {
        base.Initialise();
        currentValue = PlayerPrefs.GetString($"Options/{type}/{settingName}", defaultValue);
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(string value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetString($"Options/{type}/{settingName}", value);

        Save();
    }

    public string GetCurrentValue()
    {
        return currentValue;
    }
}
