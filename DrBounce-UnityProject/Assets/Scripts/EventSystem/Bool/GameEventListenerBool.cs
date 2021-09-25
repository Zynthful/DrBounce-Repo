using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerBool : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventBool Event;

    [Tooltip("What should be executed when the event is raised")]
    public BoolEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(bool value)
    {
        Response?.Invoke(value);
    }
}
