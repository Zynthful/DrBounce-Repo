using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Setting", menuName = "ScriptableObjects/Settings/Float")]
public class FloatSetting : GenericSettingObj
{
    [SerializeField]
    protected GameEventFloat onValueChanged = null;

    [SerializeField]
    protected float minValue = 0;
    [SerializeField]
    protected float maxValue = 1;
    [SerializeField]
    protected float defaultValue = 0;

    protected float currentValue = 0;

    public override void Initialise()
    {
        base.Initialise();
        currentValue = PlayerPrefs.GetFloat($"Options/{type}/{settingName}", defaultValue);
        onValueChanged?.Raise(currentValue);
    }

    public virtual void SetValue(float value)
    {
        currentValue = value;
        onValueChanged?.Raise(value);
        PlayerPrefs.SetFloat($"Options/{type}/{settingName}", value);

        Save();
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
