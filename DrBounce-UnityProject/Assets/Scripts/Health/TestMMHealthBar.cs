using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

/// <summary>
/// A simple class used to interact with a MMHealthBar component and test it
/// To use it, add it to an object with a MMHealthBar, and at runtime, move its CurrentHealth slider, and press the Test button to update the bar
/// </summary>
public class TestMMHealthBar : MonoBehaviour
{
    [Range(0f, 100f)]
    public float CurrentHealth = 50f;

    protected float _minimumHealth = 0f;
    protected float _maximumHealth = 100f;
    protected MMHealthBar _targetHealthBar;

    [MMInspectorButton("Test")] public bool TestButton;

    protected virtual void Awake()
    {
        _targetHealthBar = this.gameObject.GetComponent<MMHealthBar>();
    }

    public virtual void Test()
    {
        if (_targetHealthBar != null)
        {
            _targetHealthBar.UpdateBar(CurrentHealth, _minimumHealth, _maximumHealth, true);
        }
    }
}