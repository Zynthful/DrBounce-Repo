using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownSetting : Settings
{
    [SerializeField]
    private IntSetting setting = null;

    [SerializeField]
    private TMP_Dropdown dropdown = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        dropdown.onValueChanged.AddListener(setting.SetValue);
        setting.OnResetDefault += ResetToDefault;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        dropdown.onValueChanged.RemoveListener(setting.SetValue);
        setting.OnResetDefault -= ResetToDefault;
    }

    protected override void UpdateUI()
    {
        base.UpdateUI();
        dropdown.value = setting.GetCurrentValue();
    }
}
