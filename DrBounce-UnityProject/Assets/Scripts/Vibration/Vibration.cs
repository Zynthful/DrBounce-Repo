using UnityEngine;
using MoreMountains.NiceVibrations;

public abstract class Vibration : ScriptableObject
{
    [Header("Base Vibration Settings")]
    [SerializeField]
    [Tooltip("Type of haptic to trigger.")]
    protected HapticTypes type = HapticTypes.None;
    [SerializeField]
    [Tooltip("Whether to also trigger rumbling.")]
    protected bool alsoRumble = false;
    [SerializeField]
    [Tooltip("Controller ID to trigger this vibration on.")]
    protected int controllerID = -1;

    [Space]
    [SerializeField]
    [Tooltip("[OPTIONAL] The events that will trigger this vibration. Only supports null events : (")]
    protected GameEvent[] triggerEvents = null;

    protected virtual void OnEnable()
    {
        if (triggerEvents != null)
        {
            for (int i = 0; i < triggerEvents.Length; i++)
            {
                triggerEvents[i].RegisterListener(Trigger);
            }
        }
    }

    protected virtual void OnDisable()
    {
        if (triggerEvents != null)
        {
            for (int i = 0; i < triggerEvents.Length; i++)
            {
                triggerEvents[i].UnregisterListener(Trigger);
            }
        }
    }

    public virtual void Trigger()
    {
        // doesn't work lol
        /*
        // Prevent triggering if a continuous vibration is already active
        // (this stops continuous vibrations from stopping)
        if (VibrationManager.activeContinuousVibrations.Count >= 1)
            return;
        */
    }

    public virtual void StopAll()
    {
        MMVibrationManager.StopAllHaptics();
    }
}