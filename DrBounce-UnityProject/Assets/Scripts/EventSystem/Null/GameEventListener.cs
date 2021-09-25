using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEvent Event;

    [Tooltip("What should be executed when the event is raised")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }
    
    public void OnEventRaised()
    {
        Response?.Invoke();
    }
}
