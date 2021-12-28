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

    protected override void Start()
    {
        base.Start();
        dropdown.value = setting.GetCurrentValue();
    }

    protected virtual void OnEnable()
    {
        dropdown.onValueChanged.AddListener(setting.SetValue);
    }

    protected virtual void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(setting.SetValue);
    }
}
