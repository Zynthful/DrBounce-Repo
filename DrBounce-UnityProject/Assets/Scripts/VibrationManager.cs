using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class VibrationManager : MonoBehaviour
{

    public HapticTypes dashHapticType = HapticTypes.LightImpact;
    // Start is called before the first frame update
 
    public void DashVibration()
    {
        MMVibrationManager.Haptic(dashHapticType, false, true, this);
    }

}
