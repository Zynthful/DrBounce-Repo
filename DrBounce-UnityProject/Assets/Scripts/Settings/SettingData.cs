using System;
using UnityEngine;

public abstract class SettingData : ScriptableObject
{
    [Header("Base Setting Info")]
    [SerializeField]
    protected string settingName = null;

    [SerializeField]
    protected SettingType type = new SettingType();
    public SettingType GetSettingType() { return type; }

    [SerializeField]
    protected SettingSubType subType = new SettingSubType();
    public SettingSubType GetSettingSubType() { return subType; }

    public delegate void ResetDefault();
    public event ResetDefault OnResetDefault;

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
}