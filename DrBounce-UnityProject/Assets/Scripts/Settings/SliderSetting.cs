using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : Settings
{
    [SerializeField]
    protected FloatSetting setting = null;

    [SerializeField]
    protected Slider slider = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        slider.onValueChanged.AddListener(setting.SetValue);
        setting.OnResetDefault += ResetToDefault;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        slider.onValueChanged.RemoveListener(setting.SetValue);
        setting.OnResetDefault -= ResetToDefault;
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        valueText.text = setting.GetCurrentValue().ToString();
        slider.minValue = setting.GetMinValue();
        slider.maxValue = setting.GetMaxValue();
        slider.value = setting.GetCurrentValue();
    }
}