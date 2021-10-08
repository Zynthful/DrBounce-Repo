using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class VibrationManager : MonoBehaviour
{

    public HapticTypes dashHapticType = HapticTypes.LightImpact;
    public HapticTypes shootingHapticType = HapticTypes.LightImpact;
    public HapticTypes ChargedShootingHapticType = HapticTypes.LightImpact;
    public HapticTypes TakeDamageHapticType = HapticTypes.LightImpact;
    public HapticTypes CatchingHapticType = HapticTypes.LightImpact;
    // Start is called before the first frame update

    public void DashVibration()
    {
        MMVibrationManager.Haptic(dashHapticType, false, true, this);
    }

    public void BasicShotVibration()
    {
        MMVibrationManager.Haptic(shootingHapticType, false, true, this);
    }

    public void ChargedShotVibration()
    {
        MMVibrationManager.Haptic(ChargedShootingHapticType, false, true, this);
    }

    public void TakeDamageVibration()
    {
        MMVibrationManager.Haptic(TakeDamageHapticType, false, true, this);
    }

    public void CatchVibration()
    {
        MMVibrationManager.Haptic(CatchingHapticType, false, true, this);
    }
}
