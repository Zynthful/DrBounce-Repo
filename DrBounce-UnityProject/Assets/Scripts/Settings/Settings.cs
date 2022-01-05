using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Settings : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI valueText = null;

    protected virtual void Awake()
    {
        UpdateUI();
    }

    protected virtual void OnEnable()
    {
        UpdateUI();
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void UpdateUI()
    {

    }

    protected virtual void ResetToDefault()
    {
        UpdateUI();
    }

    /*
    private bool hasUnsavedChanges = false;

    public bool CheckUnsavedChanges()
    {
        return hasUnsavedChanges;
    }
    */
}
