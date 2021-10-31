using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : SliderSetting
{
    protected override void SetValue(float value)
    {
        base.SetValue(value);
        AkSoundEngine.SetRTPCValue(settingName, value);
    }
}