using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseRTPC : MonoBehaviour
{
    [Header("RTPC Settings")]
    [SerializeField]
    private AK.Wwise.RTPC rtpc = null;
    [SerializeField]
    [Tooltip("The gameObject that the RTPC is assigned to")]
    private GameObject assignedObject = null;

    [Header("Starting Value (OPTIONAL)")]
    [SerializeField]
    [Tooltip("If enabled, this RTPC will be set on start using the starting value.")]
    private bool setValueOnStart = false;
    [SerializeField]
    [Tooltip("The starting value that the RTPC will be set, if enabled.")]
    private float startingValue = 0.0f;

    private void Start()
    {
        if (setValueOnStart)
        {
            SetValue(startingValue);
        }
    }

    public void SetValue(float value)
    {
        if (assignedObject != null)
        {
            rtpc.SetValue(assignedObject, value);
        }
        else
        {
            rtpc.SetValue(gameObject, value);
        }
    }

    public void SetValue(int value)
    {
        SetValue((float) value);
    }

    public void SetGlobalValue(float value)
    {
        rtpc.SetGlobalValue(value);
    }

    public float GetValue(GameObject gameObject)
    {
        return rtpc.GetValue(gameObject);
    }

    public float GetValue()
    {
        return rtpc.GetValue(assignedObject);
    }

    public float GetGlobalValue()
    {
        return rtpc.GetGlobalValue();
    }
}