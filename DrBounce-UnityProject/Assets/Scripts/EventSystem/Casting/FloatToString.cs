using UnityEngine;
using UnityEngine.Events;

public class FloatToString : MonoBehaviour
{
    [SerializeField]
    private StringEvent onConversion = null;

    public void InvokeConversion(float value) => onConversion?.Invoke(value.ToString());
    public void InvokeRoundedConversion(float value) => onConversion?.Invoke(Mathf.RoundToInt(value).ToString());
}