using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private SettingData[] settings = null;

    private void Awake()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            settings[i].Initialise();
        }
    }

    public void ResetAll()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            settings[i].ResetToDefault();
        }
    }

     public void ResetAllOfType(SettingData.SettingType type)
     {
        for (int i = 0; i < settings.Length; i++)
        {
            if (settings[i].GetSettingType() == type)
            {
                settings[i].ResetToDefault();
            }
        }
    }

    public void ResetAllOfType(SettingData.SettingSubType type)
    {
        for (int i = 0; i < settings.Length; i++)
        {
            if (settings[i].GetSettingSubType() == type)
            {
                settings[i].ResetToDefault();
            }
        }
    }
}
