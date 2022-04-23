using UnityEngine;
using UnityEngine.Events;

public class CompareBoolInvoke : MonoBehaviour
{
    public UnityEvent True;
    public UnityEvent False;

    public void Compare(bool value)
    {
        if (value)
            True.Invoke();
        else
            False.Invoke();
    }
}