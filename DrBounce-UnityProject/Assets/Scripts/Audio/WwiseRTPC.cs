using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseRTPC : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.RTPC rtpc = null;

    [SerializeField]
    [Tooltip("The gameObject that the RTPC is assigned to")]
    private GameObject assignedObject = null;

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