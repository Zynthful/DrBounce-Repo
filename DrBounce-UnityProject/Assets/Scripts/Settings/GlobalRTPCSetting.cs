using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Global RTPC Setting", menuName = "ScriptableObjects/Settings/GlobalRTPC")]
public class GlobalRTPCSetting : FloatSetting
{
    [Header("RTPC")]
    [SerializeField]
    protected AK.Wwise.RTPC rtpc = null;

    public override void Initialise()
    {
        base.Initialise();
        rtpc.SetGlobalValue(currentValue);
    }

    public override void SetValue(float value)
    {
        base.SetValue(value);
        rtpc.SetGlobalValue(value);
        onResetToDefault?.Raise(defaultValue);
    }

    public override void ResetToDefault()
    {
        SetValue(defaultValue);
        base.ResetToDefault();
    }
}
