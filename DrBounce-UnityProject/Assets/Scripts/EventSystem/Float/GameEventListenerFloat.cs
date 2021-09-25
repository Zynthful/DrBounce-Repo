using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerFloat : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventFloat Event;

    [Tooltip("What should be executed when the event is raised")]
    public FloatEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(float value)
    {
        Response?.Invoke(value);
    }
}
