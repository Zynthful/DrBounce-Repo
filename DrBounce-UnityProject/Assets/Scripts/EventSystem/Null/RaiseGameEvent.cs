using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseGameEvent : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent = null;

    public void Raise()
    {
        gameEvent?.Raise();
    }
}
