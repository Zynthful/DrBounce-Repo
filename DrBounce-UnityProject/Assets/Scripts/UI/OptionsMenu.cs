using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Events")]

    // Passes mouse sensitivity value
    [SerializeField]
    private GameEventFloat onMouseSensChange = null;

    // Passes controller sensitivity value
    [SerializeField]
    private GameEventFloat onControllerSensChange = null;

    // Passes crouch mode: 1 = Toggle, 0 = Hold - this is an int because Unity's dropdowns pass ints through OnValueChanged()
    [SerializeField]
    private GameEventInt onIsCrouchToggle = null;

    /*
    private bool hasUnsavedChanges = false;

    public bool CheckUnsavedChanges()
    {
        return hasUnsavedChanges;
    }
    */

    public void SetMouseSensitivity(float value)
    {
        onMouseSensChange?.Raise(value);
        PlayerPrefs.SetFloat("Options/MouseSensitivity", value);
        // hasUnsavedChanges = true;

        Save();
    }

    public void SetControllerSensitivity(float value)
    {
        onControllerSensChange?.Raise(value);
        PlayerPrefs.SetFloat("Options/ControllerSensitivity", value);
        // hasUnsavedChanges = true;

        Save();
    }
    public void SetCrouchMode(int value)
    {
        onIsCrouchToggle?.Raise(value);
        PlayerPrefs.SetInt("Options/CrouchMode", value);
        // hasUnsavedChanges = true;

        Save();
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }

}
