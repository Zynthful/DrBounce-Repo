using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSwitch : MonoBehaviour
{
    [Header("Switch Settings")]
    [SerializeField]
    private AK.Wwise.Switch @switch = null;
    [SerializeField]
    [Tooltip("The gameObject that the Switch is assigned to. If this is null, it will use this game object.")]
    private GameObject assignedObject = null;

    [Header("Starting Value (OPTIONAL)")]
    [SerializeField]
    [Tooltip("If enabled, this Switch will be set on start.")]
    private bool setValueOnStart = false;

    private void Start()
    {
        if (setValueOnStart)
        {
            @switch.SetValue(assignedObject == null ? gameObject : assignedObject);
        }
    }
}
