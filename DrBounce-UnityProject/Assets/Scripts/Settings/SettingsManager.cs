using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private IntSetting[] intSettings = null;

    [SerializeField]
    private FloatSetting[] floatSettings = null;

    [SerializeField]
    private GlobalRTPCSetting[] rtpcSettings = null;

    private void Start()
    {
        for (int i = 0; i < intSettings.Length; i++)
        {
            intSettings[i].Initialise();
        }
        for (int i = 0; i < floatSettings.Length; i++)
        {
            floatSettings[i].Initialise();
        }
        for (int i = 0; i < rtpcSettings.Length; i++)
        {
            rtpcSettings[i].Initialise();
        }
    }
}
