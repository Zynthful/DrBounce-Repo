using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event Enemy", menuName = "ScriptableObjects/Events/Game Event Enemy")]
public class GameEventEnemy : ScriptableObject
{
    private List<GameEventListenerEnemy> listeners = new List<GameEventListenerEnemy>();

    public void Raise(Enemy value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(GameEventListenerEnemy listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerEnemy listener)
    {
        listeners.Remove(listener);
    }
}
