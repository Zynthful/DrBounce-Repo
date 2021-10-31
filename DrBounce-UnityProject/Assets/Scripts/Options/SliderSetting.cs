using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : Settings
{
    [SerializeField]
    protected Slider slider = null;

    [SerializeField]
    protected GameEventFloat onValueChanged = null;

    protected float initialValue = 0;

    protected virtual void Start()
    {
        initialValue = PlayerPrefs.GetFloat($"Options/{type}/{settingName}", slider.value);
        onValueChanged?.Raise(initialValue);
        slider.value = initialValue;
    }

    protected virtual void OnEnable()
    {
        slider.onValueChanged.AddListener(SetValue);
    }

    protected virtual void OnDisable()
    {
        slider.onValueChanged.RemoveListener(SetValue);
    }

    protected virtual void SetValue(float value)
    {
        onValueChanged?.Raise(value);
        PlayerPrefs.SetFloat($"Options/{type}/{settingName}", value);

        Save();
    }
}