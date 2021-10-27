using UnityEngine;
using UnityEngine.Events;

public class IntToString : MonoBehaviour
{
    [SerializeField]
    private StringEvent onConversion = null;

    public void InvokeConversion(int value) => onConversion?.Invoke(value.ToString());
}