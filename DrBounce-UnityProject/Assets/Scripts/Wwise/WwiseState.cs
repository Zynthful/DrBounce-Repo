using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseState : MonoBehaviour
{
    [Header("State Settings")]
    [SerializeField]
    private AK.Wwise.State state = null;
    public void SetState(AK.Wwise.State value) { state = value; }
    public AK.Wwise.State GetState() { return state; }

    [Header("Starting Value (OPTIONAL)")]
    [SerializeField]
    [Tooltip("If enabled, this state will be set on start.")]
    private bool setValueOnStart = false;

    private void Start()
    {
        if (setValueOnStart)
        {
            Invoke();
        }
    }

    public void Invoke()
    {
        try
        {
            state.SetValue();
        }
        catch
        {
            Debug.LogError($"uh oh, you are trying to invoke this state: {this}, but the state has not been set! (ask jamie)");
        }
    }
}
