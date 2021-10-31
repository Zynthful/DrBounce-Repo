using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : SliderSetting
{
    [SerializeField]
    private AK.Wwise.RTPC rtpc = null;

    protected override void Start()
    {
        base.Start();
        SetValue(initialValue);
    }

    protected override void SetValue(float value)
    {
        base.SetValue(value);
        rtpc.SetGlobalValue(value);
    }
}