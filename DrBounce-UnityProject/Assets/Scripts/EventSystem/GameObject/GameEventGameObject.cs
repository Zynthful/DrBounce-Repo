using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event GameObject", menuName = "ScriptableObjects/Events/Game Event GameObject")]
public class GameEventGameObject : ScriptableObject
{
    private List<GameEventListenerGameObject> listeners = new List<GameEventListenerGameObject>();

    public void Raise(GameObject obj)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(obj);
        }
    }

    public void RegisterListener(GameEventListenerGameObject listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerGameObject listener)
    {
        listeners.Remove(listener);
    }
}
