using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Setting", menuName = "ScriptableObjects/Settings/Int")]
public class IntSetting : SettingData
{
    [SerializeField]
    private GameEventInt onValueChanged = null;

    [SerializeField]
    protected int minValue = 0;
    [SerializeField]
    protected int maxValue = 1;
    [SerializeField]
    protected int defaultValue = 0;

    protected int currentValue = 0;

    public override void Initialise()
    {
        base.Initialise();
        currentValue = PlayerPrefs.GetInt($"Options/{type}/{settingName}", defaultValue);
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(int value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetInt($"Options/{type}/{settingName}", value);

        Save();
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
