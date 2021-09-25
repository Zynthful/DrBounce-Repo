using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerString : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventString Event;

    [Tooltip("What should be executed when the event is raised")]
    public StringEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(string value)
    {
        Response?.Invoke(value);
    }
}
