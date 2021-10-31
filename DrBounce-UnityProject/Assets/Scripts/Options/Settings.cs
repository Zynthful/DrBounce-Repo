using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
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

    public void Save()
    {
        PlayerPrefs.Save();
    }

    /*
    private bool hasUnsavedChanges = false;

    public bool CheckUnsavedChanges()
    {
        return hasUnsavedChanges;
    }
    */
}
