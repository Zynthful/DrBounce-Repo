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

    protected override void Start()
    {
        base.Start();
        valueText.text = setting.GetCurrentValue().ToString();
        slider.minValue = setting.GetMinValue();
        slider.maxValue = setting.GetMaxValue();
        slider.value = setting.GetCurrentValue();
    }

    protected virtual void OnEnable()
    {
        slider.onValueChanged.AddListener(setting.SetValue);
    }

    protected virtual void OnDisable()
    {
        slider.onValueChanged.RemoveListener(setting.SetValue);
    }
}