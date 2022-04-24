using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

[CreateAssetMenu(fileName = "Continuous Vibration", menuName = "ScriptableObjects/Vibrations/Continuous Vibration")]
public class ContinuousVibration : Vibration
{
    [Header("Continuous Settings")]
    [SerializeField]
    [Tooltip("Intensity of the vibration between 0 and 1.")]
    [Range(0, 1)]
    protected float intensity = 0.0f;
    [SerializeField]
    [Tooltip("Sharpness of the vibration between 0 and 1.")]
    [Range(0, 1)]
    protected float sharpness = 0.0f;
    [SerializeField]
    [Tooltip("Duration of the vibration in seconds.")]
    protected float duration = 0.0f;
    [SerializeField]
    [Tooltip("Whether to call this on the main thread (true) or a secondary thread (false).")]
    protected bool threaded = false;
    [SerializeField]
    [Tooltip("Whether to allow for full intensity control for subsequent updates.")]
    protected bool fullIntensity = false;

    [Space]
    [SerializeField]
    [Tooltip("[OPTIONAL] The events that will stop all continuous vibrations (including rumble). Only supports null events : (")]
    protected GameEvent[] stopEvents = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < stopEvents.Length; i++)
        {
            stopEvents[i].RegisterListener(Stop);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < stopEvents.Length; i++)
        {
            stopEvents[i].UnregisterListener(Stop);
        }
    }

    public override void Trigger()
    {
        base.Trigger();
        VibrationManager.activeContinuousVibrations.Add(this);
        MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration, HapticTypes.None, GameManager.s_Instance, alsoRumble, controllerID, threaded, fullIntensity);
    }

    public virtual void Stop(bool alsoStopRumble)
    {
        if (!VibrationManager.activeContinuousVibrations.Contains(this))
            return;

        MMVibrationManager.StopContinuousHaptic(alsoStopRumble);
        List<ContinuousVibration> activeConts = new List<ContinuousVibration>(VibrationManager.activeContinuousVibrations);
        VibrationManager.activeContinuousVibrations.Clear();

        // Play the last triggered continuous vibration if we're stopping multiple, as there's no way to stop specific vibrations
        if (activeConts.Count > 1)
        {
            ContinuousVibration lastCont = activeConts[activeConts.Count - 1];
            lastCont.Trigger();
        }
    }

    public virtual void Stop()
    {
        Stop(true);
    }
}
