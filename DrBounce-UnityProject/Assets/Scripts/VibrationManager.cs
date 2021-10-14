using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class VibrationManager : MonoBehaviour
{
    //List of vibration Haptic types
    
    [Header("Movement")]
    public HapticTypes dashHapticType = HapticTypes.None;
    public HapticTypes jumpHapticType = HapticTypes.None;
    [Header("Shooting")]
    public HapticTypes shootingHapticType = HapticTypes.None;
    public HapticTypes ChargedShootingHapticType = HapticTypes.None;
    [SerializeField] float ChargeIntensity;
    [SerializeField] float ChargeSharpness;
    [Header("Misc")]
    public HapticTypes TakeDamageHapticType = HapticTypes.None;
    public HapticTypes CatchingHapticType = HapticTypes.None;
    [Header("Stop")]
    public HapticTypes StopMagnetHapticType = HapticTypes.None;
    public HapticTypes StopActiveHapticType = HapticTypes.None;
    [SerializeField] float MagnetIntensity;
    [SerializeField] float MagnetSharpness;
    //public HapticTypes activeChargeHapticType = HapticTypes.None;
    //public HapticTypes magnetAimAssistHapticType = HapticTypes.None;
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

    public void JumpVibration()
    {
        MMVibrationManager.Haptic(jumpHapticType, false, true, this);
    }

    public void MagnetAimAssitVibration()
    {
        MMVibrationManager.ContinuousHaptic(0.3f, 0.8f, 3.5f, HapticTypes.None, this, true);
    }

    public void ActiveChargeVibration()
    {
        MMVibrationManager.ContinuousHaptic(ChargeIntensity,ChargeSharpness, 3.5f, HapticTypes.None, this, true);
    }

    public void ActiveMagnetAssist()
    {
        MMVibrationManager.ContinuousHaptic(MagnetIntensity, MagnetSharpness, 3.5f, HapticTypes.None, this, true);
    }

    public void StopMagnet()
    {
        MMVibrationManager.Haptic(StopMagnetHapticType, false, true, this);

    }

    public void StopActiveCharge()
    {
        MMVibrationManager.Haptic(StopActiveHapticType, false, true, this);
    }
}
