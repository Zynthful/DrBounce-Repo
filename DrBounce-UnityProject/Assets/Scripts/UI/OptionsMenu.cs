using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private GameEventFloat onMouseSensChange = null;

    private void Start()
    {
        
    }

    public void SetSensitivity(float value)
    {
        onMouseSensChange?.Raise(value);
        // PlayerPrefs.SetFloat("Mouse Sensitivity", value);
    }
}
