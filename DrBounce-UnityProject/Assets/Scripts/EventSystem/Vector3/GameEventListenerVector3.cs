using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerVector3 : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventVector3 Event;

    [Tooltip("What should be executed when the event is raised")]
    public Vector3Event Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Vector3 value)
    {
        Response?.Invoke(value);
    }
}
