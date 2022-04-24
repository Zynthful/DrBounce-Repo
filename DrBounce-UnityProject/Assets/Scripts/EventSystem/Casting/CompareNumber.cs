using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompareNumber : MonoBehaviour
{
    [SerializeField]
    private float compareValue = 0;
    public void SetCompareValue(float value) { compareValue = value; }

    public UnityEvent<float> Equal;
    public UnityEvent<float> GreaterThan;
    public UnityEvent<float> GreaterOrEqualTo;
    public UnityEvent<float> LessThan;
    public UnityEvent<float> LessOrEqualTo;

    public void Compare(float value)
    {
        if (value == compareValue)
        {
            Equal.Invoke(value);
            GreaterOrEqualTo.Invoke(value);
            LessOrEqualTo.Invoke(value);
        }
        else if (value > compareValue)
        {
            GreaterThan.Invoke(value);
            GreaterOrEqualTo.Invoke(value);
        }
        else
        {
            LessThan.Invoke(value);
            LessOrEqualTo.Invoke(value);
        }
    }

    public void Compare(int value)
    {
        Compare((float)value);
    }
}