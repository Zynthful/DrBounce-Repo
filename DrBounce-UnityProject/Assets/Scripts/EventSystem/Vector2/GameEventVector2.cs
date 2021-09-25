using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event Vector2", menuName = "ScriptableObjects/Events/Game Event Vector2")]
public class GameEventVector2 : ScriptableObject
{
    private List<GameEventListenerVector2> listeners = new List<GameEventListenerVector2>();

    public void Raise(Vector2 value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(value);
        }
    }

    public void RegisterListener(GameEventListenerVector2 listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerVector2 listener)
    {
        listeners.Remove(listener);
    }
}
