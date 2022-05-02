using UnityEngine;
using MoreMountains.NiceVibrations;

[CreateAssetMenu(fileName = "Single Vibration", menuName = "ScriptableObjects/Vibrations/Single Vibration")]
public class SingleVibration : Vibration
{
    [Header("Single Settings")]
    [SerializeField]
    protected bool defaultToRegularVibrate = false;

    public override void Trigger()
    {
        base.Trigger();
        MMVibrationManager.Haptic(type, defaultToRegularVibrate, alsoRumble, GameManager.s_Instance, controllerID);
    }
}