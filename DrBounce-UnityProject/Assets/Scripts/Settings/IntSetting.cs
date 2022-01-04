using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Setting", menuName = "ScriptableObjects/Settings/Int")]
public class IntSetting : SettingData
{
    protected int currentValue = 0;

    [Header("Events")]
    [SerializeField]
    protected GameEventInt onValueChanged = null;
    [SerializeField]
    protected new GameEventInt onResetToDefault = null;

    [Header("Value Settings")]
    [SerializeField]
    protected int minValue = 0;
    [SerializeField]
    protected int maxValue = 1;
    [SerializeField]
    protected int defaultValue = 0;

    public override void Initialise()
    {
        base.Initialise();
        currentValue = PlayerPrefs.GetInt($"Options/{type}/{subType}/{settingName}", defaultValue);
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(int value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetInt($"Options/{type}/{subType}/{settingName}", value);

        Save();
    }

    public override void ResetToDefault()
    {
        SetValue(defaultValue);
        onResetToDefault?.Raise(defaultValue);
        base.ResetToDefault();
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }
    public int GetMinValue()
    {
        return minValue;
    }
    public float GetMaxValue()
    {
        return maxValue;
    }
}
