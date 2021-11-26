using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Settings : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI valueText = null;

    protected virtual void Start()
    {

    }

    /*
    private bool hasUnsavedChanges = false;

    public bool CheckUnsavedChanges()
    {
        return hasUnsavedChanges;
    }
    */
}
