using UnityEngine;
using MoreMountains.NiceVibrations;

[CreateAssetMenu(fileName = "Continuous Vibration", menuName = "ScriptableObjects/Vibrations/Continuous Vibration")]
public class ContinuousVibration : Vibration
{
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

    public override void Trigger()
    {
        base.Trigger();
        MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration, HapticTypes.None, GameManager.s_Instance, alsoRumble, controllerID, threaded, fullIntensity);
    }
}
