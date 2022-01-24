using System;
using UnityEngine;

public abstract class SettingData : ScriptableObject
{
    public enum SettingType
    {
        Game,
        Video,
        Audio,
        Controls,
    }

    public enum SettingSubType
    {
        Sensitivity,
        Movement,
        Volume,
        ActionBindingKeyboard,
        ActionBindingController,
        Debug,
    }

    [Header("Base Setting Info")]
    [SerializeField]
    protected string settingName = null;

    [SerializeField]
    protected SettingType type = new SettingType();
    [SerializeField]
    protected SettingSubType subType = new SettingSubType();

    public delegate void ResetDefault();
    public event ResetDefault OnResetDefault;

    protected void Save()
    {
        PlayerPrefs.Save();
    }

    public virtual void Initialise()
    {

    }

    public virtual void ResetToDefault()
    {
        if (OnResetDefault != null)
        {
            OnResetDefault();
        }
    }

    public SettingType GetSettingType()
    {
        return type;
    }

    public SettingSubType GetSettingSubType()
    {
        return subType;
    }
}
