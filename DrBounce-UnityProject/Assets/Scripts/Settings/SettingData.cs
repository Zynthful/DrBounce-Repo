using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettingData : ScriptableObject
{
    protected enum SettingType
    {
        Sensitivity,
        Movement,
        Volume,
    }

    [SerializeField]
    protected string settingName = null;

    [SerializeField]
    protected SettingType type = SettingType.Sensitivity;

    protected void Save()
    {
        PlayerPrefs.Save();
    }

    public virtual void Initialise()
    {

    }
}
