using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Delay : MonoBehaviour
{
    [Header("Delay Settings")]
    [SerializeField]
    private float duration = 0.0f;

    [Header("Events")]
    public UnityEvent onFinish;

    public void InvokeDelay(bool realTime)
    {
        StartCoroutine(TheDelay(realTime));
    }

    private IEnumerator TheDelay(bool realTime)
    {
        if (realTime)
            yield return new WaitForSecondsRealtime(duration);
        else
            yield return new WaitForSeconds(duration);
    }
}