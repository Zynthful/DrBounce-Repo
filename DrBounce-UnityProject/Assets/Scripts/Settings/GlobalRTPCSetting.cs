using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Global RTPC Setting", menuName = "ScriptableObjects/Settings/GlobalRTPC")]
public class GlobalRTPCSetting : FloatSetting
{
    [SerializeField]
    private AK.Wwise.RTPC rtpc = null;

    public override void Initialise()
    {
        base.Initialise();
        rtpc.SetGlobalValue(currentValue);
    }

    public override void SetValue(float value)
    {
        base.SetValue(value);
        rtpc.SetGlobalValue(value);
    }
}
