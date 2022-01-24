using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : SliderSetting
{
    [SerializeField]
    protected GlobalRTPCSetting rtpcSetting = null;

    protected override void Awake()
    {
        setting = rtpcSetting;
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        slider.onValueChanged.AddListener(rtpcSetting.SetValue);
    }   

    protected override void OnDisable()
    {
        base.OnEnable();
        slider.onValueChanged.RemoveListener(rtpcSetting.SetValue);
    }
}