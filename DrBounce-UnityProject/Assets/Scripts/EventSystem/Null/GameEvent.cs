using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Game Event", menuName = "ScriptableObjects/Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    private List<UnityAction> calls = new List<UnityAction>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
        for (int i = 0; i < calls.Count; i++)
        {
            calls[i].Invoke();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RegisterListener(UnityAction call)
    {
        calls.Add(call);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
    
    public void UnregisterListener(UnityAction call)
    {
        calls.Remove(call);
    }
}
