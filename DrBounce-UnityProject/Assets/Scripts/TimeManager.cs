using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    private static float lastTimeScale = 1.0f;

    public static IEnumerator SetTimeScaleForDuration(float timeScale, float duration, bool realtime = true)
    {
        SetTimeScale(timeScale);
        if (realtime)
            yield return new WaitForSecondsRealtime(duration);
        else
            yield return new WaitForSeconds(duration);
        SetTimeScale(lastTimeScale);
    }

    public static void SetTimeScaleForDuration(float timeScale, float duration, MonoBehaviour behaviour, bool realtime = true)
    {
        behaviour.StartCoroutine(SetTimeScaleForDuration(timeScale, duration, realtime));
    }

    public static void FreezeFrame(float duration, MonoBehaviour behaviour)
    {
        behaviour.StartCoroutine(SetTimeScaleForDuration(0.0f, duration, true)); 
    }

    public static void SetTimeScale(float timeScale)
    {
        lastTimeScale = Time.timeScale;
        Time.timeScale = timeScale;
    }

    public static void SetLastTimeScale()
    {
        SetTimeScale(lastTimeScale);
    }
}