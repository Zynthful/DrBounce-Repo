using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerVector2 : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventVector2 Event;

    [Tooltip("What should be executed when the event is raised")]
    public Vector2Event Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Vector2 value)
    {
        Response?.Invoke(value);
    }
}
