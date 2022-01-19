using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bool Setting", menuName = "ScriptableObjects/Settings/Bool Setting")]
public class BoolSetting : SettingData
{
    protected bool currentValue = false;

    [Header("Events")]
    [SerializeField]
    protected GameEventBool onValueChanged = null;
    [SerializeField]
    protected GameEventBool onResetToDefault = null;

    [Header("Value Settings")]
    [SerializeField]
    protected bool defaultValue = false;

    public override void Initialise()
    {
        base.Initialise();
        int defaultVal = defaultValue ? 1 : 0;
        int currentVal = PlayerPrefs.GetInt($"Options/{type}/{subType}/{settingName}", defaultVal);
        currentValue = currentVal == 1 ? true : false;
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(bool value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetInt($"Options/{type}/{subType}/{settingName}", value ? 1 : 0);   // PlayerPrefs only stores ints, so we pass 1 or 0 based on the bool value

        Save();
    }

    public override void ResetToDefault()
    {
        SetValue(defaultValue);
        onResetToDefault?.Raise(defaultValue);
        base.ResetToDefault();
    }

    public bool GetCurrentValue()
    {
        return currentValue;
    }

    public void ToggleValue()
    {
        SetValue(!currentValue);
    }
}
