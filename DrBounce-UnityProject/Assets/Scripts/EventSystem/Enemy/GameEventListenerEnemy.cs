using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerEnemy : MonoBehaviour
{
    [Tooltip("The event which this object is listening for")]
    public GameEventEnemy Event;

    [Tooltip("What should be executed when the event is raised")]
    public EnemyEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(Enemy value)
    {
        Response?.Invoke(value);
    }
}
