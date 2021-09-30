using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertBool : MonoBehaviour
{
    [SerializeField]
    private BoolEvent onConversion = null;

    public void InvokeInversion(bool value) => onConversion?.Invoke(!value);
}
