using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerInt : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventInt Event;

    [Tooltip("What should be executed when the event is raised")]
    public IntEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(int value)
    {
        Response?.Invoke(value);
    }
}
