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
    // Passes whether crouch mode is Toggle (1/true) or Hold (0/false) - this is an int because Unity's dropdowns pass ints through OnValueChanged()
    [SerializeField]
    private GameEventInt onIsCrouchToggle = null;

    private void Start()
    {
        
    }

    public void SetSensitivity(float value)
    {
        onMouseSensChange?.Raise(value);
        // PlayerPrefs.SetFloat("Mouse Sensitivity", value);
    }

    public void SetCrouchMode(int value)
    {
        onIsCrouchToggle?.Raise(value);
    }
}
