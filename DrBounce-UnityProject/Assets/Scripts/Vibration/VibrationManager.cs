using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class VibrationManager : MonoBehaviour
{
    [HideInInspector]
    public static List<ContinuousVibration> activeContinuousVibrations = new List<ContinuousVibration>();

    public static void SetAllPaused(bool value)
    {
        for (int i = 0; i < activeContinuousVibrations.Count; i++)
        {
            activeContinuousVibrations[i].SetPaused(value);
        }
    }

    public static void PauseAll() { SetAllPaused(true); }

    public static void ResumeAll() { SetAllPaused(false); }
}