using UnityEngine;
using MoreMountains.NiceVibrations;

public abstract class Vibration : ScriptableObject
{
    [SerializeField]
    [Tooltip("Type of haptic to trigger.")]
    protected HapticTypes type = HapticTypes.None;
    [SerializeField]
    [Tooltip("Whether to also trigger rumbling.")]
    protected bool alsoRumble = false;
    [SerializeField]
    [Tooltip("Controller ID to trigger this vibration on.")]
    protected int controllerID = -1;

    public virtual void Trigger()
    {
        //Debug.Log("triggering vibration " + name, this);
    }

    public virtual void StopAll()
    {
        MMVibrationManager.StopAllHaptics();
    }

    public virtual void StopAllContinuous(bool alsoStopRumble)
    {
        //Debug.Log("stopping all continuous... " + name, this);
        MMVibrationManager.StopContinuousHaptic(alsoStopRumble);
    }

    public virtual void StopAllContinuous()
    {
        StopAllContinuous(true);
    }
}