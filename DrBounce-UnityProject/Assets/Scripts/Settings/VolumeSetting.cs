using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : SliderSetting
{
    [SerializeField]
    protected GlobalRTPCSetting rtpcSetting = null;

    protected override void Start()
    {
        slider.minValue = rtpcSetting.GetMinValue();
        slider.maxValue = rtpcSetting.GetMaxValue();
        slider.value = rtpcSetting.GetCurrentValue();
        valueText.text = rtpcSetting.GetCurrentValue().ToString();
    }

    protected override void OnEnable()
    {
        slider.onValueChanged.AddListener(rtpcSetting.SetValue);
    }

    protected override void OnDisable()
    {
        slider.onValueChanged.RemoveListener(rtpcSetting.SetValue);
    }
}