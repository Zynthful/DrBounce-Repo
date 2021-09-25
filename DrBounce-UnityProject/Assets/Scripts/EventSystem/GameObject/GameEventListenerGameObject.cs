using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerGameObject : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventGameObject Event;

    [Tooltip("What should be executed when the event is raised")]
    public GameObjectEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(GameObject obj)
    {
        Response?.Invoke(obj);
    }
}
