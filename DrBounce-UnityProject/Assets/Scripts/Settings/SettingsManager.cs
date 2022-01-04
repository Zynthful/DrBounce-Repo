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
}
