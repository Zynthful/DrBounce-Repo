using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class VibrationManager : MonoBehaviour
{
    [HideInInspector]
    public static List<ContinuousVibration> activeContinuousVibrations = new List<ContinuousVibration>();

    // List of vibration Haptic types

    [Header("Movement")]
    [SerializeField]
    private HapticTypes dashHapticType = HapticTypes.None;
    [SerializeField]
    private HapticTypes jumpHapticType = HapticTypes.None;

    [Header("Shooting")]
    [SerializeField]
    private HapticTypes shootingHapticType = HapticTypes.None;
    [SerializeField]
    private HapticTypes chargedShootingHapticType = HapticTypes.None;
    [SerializeField]
    private float chargeIntensity;
    [SerializeField]
    private float chargeSharpness;

    // MonoBehaviour activeChargeMono = null;

    [Header("Health")]
    [SerializeField]
    private HapticTypes takeDamageHapticType = HapticTypes.None;

    [Header("Gun Interactions")]
    [SerializeField]
    private HapticTypes catchingHapticType = HapticTypes.None;
    [SerializeField]
    private float magnetIntensity;
    [SerializeField]
    private float magnetSharpness;

    // MonoBehaviour magnetAssistMono = null;

    /*
    private void Start()
    {
        activeChargeMono = gameObject.AddComponent<MonoBehaviour>();
        magnetAssistMono = gameObject.AddComponent<MonoBehaviour>();
    }
    */

    public void DashVibration()
    {
        MMVibrationManager.Haptic(dashHapticType, false, true, this);
    }

    public void UnchargedShotVibration()
    {
        MMVibrationManager.Haptic(shootingHapticType, false, true, this);

    }

    public void ChargedShotVibration()
    {
        MMVibrationManager.Haptic(chargedShootingHapticType, false, true, this);
    }

    public void TakeDamageVibration()
    {
        MMVibrationManager.Haptic(takeDamageHapticType, false, true, this);
    }

    public void CatchVibration()
    {
        MMVibrationManager.Haptic(catchingHapticType, false, true, this);
    }

    public void JumpVibration()
    {
        MMVibrationManager.Haptic(jumpHapticType, false, true, this);
    }

    public void CheckChargeActiveVibration(bool active)
    {
        if (active)
        {
            MMVibrationManager.ContinuousHaptic(chargeIntensity, chargeSharpness, 3.5f, HapticTypes.None, this, true);
        }
        else
        {
            MMVibrationManager.StopContinuousHaptic(true);
        }
    }

    public void CheckMagnetActiveVibration(bool active)
    {
        if (active)
        {
            MMVibrationManager.ContinuousHaptic(magnetIntensity, magnetSharpness, 3.5f, HapticTypes.None, this, true);
        }
        else
        {
            MMVibrationManager.StopContinuousHaptic(true);
        }
    }
}