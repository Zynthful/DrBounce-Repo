using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSetting : Settings
{
    [SerializeField]
    protected BoolSetting setting = null;
    
    [SerializeField]
    protected Toggle toggle = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        toggle.onValueChanged.AddListener(setting.SetValue);
        setting.OnResetDefault += ResetToDefault;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        toggle.onValueChanged.RemoveListener(setting.SetValue);
        setting.OnResetDefault -= ResetToDefault;
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        toggle.isOn = setting.GetCurrentValue();
    }
}
