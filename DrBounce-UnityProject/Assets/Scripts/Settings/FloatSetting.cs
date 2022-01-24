using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Setting", menuName = "ScriptableObjects/Settings/Float")]
public class FloatSetting : SettingData
{
    protected float currentValue = 0;

    [Header("Events")]
    [SerializeField]
    protected GameEventFloat onValueChanged = null;
    [SerializeField]
    protected GameEventFloat onResetToDefault = null;

    [Header("Value Settings")]
    [SerializeField]
    protected float minValue = 0;
    [SerializeField]
    protected float maxValue = 1;
    [SerializeField]
    protected float defaultValue = 0;

    public override void Initialise()
    {
        base.Initialise();
        currentValue = PlayerPrefs.GetFloat($"Options/{type}/{subType}/{settingName}", defaultValue);
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(float value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetFloat($"Options/{type}/{subType}/{settingName}", value);

        Save();
    }

    public override void ResetToDefault()
    {
        SetValue(defaultValue);
        onResetToDefault?.Raise(defaultValue);
        base.ResetToDefault();
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }
    public float GetMinValue()
    {
        return minValue;
    }
    public float GetMaxValue()
    {
        return maxValue;
    }
}
