using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class SelectableNavigation : MonoBehaviour
{
    protected List<Selectable> selectables = null;

    protected virtual void Start()
    {
        #if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            FindNavigation();
        }
        #else
        FindNavigation();
        #endif
    }

    protected virtual List<Selectable> FindSelectables()
    {
        List<Selectable> selectables = new List<Selectable>();
        foreach (Selectable selectable in transform.GetComponentsInChildren<Selectable>())
        {
            selectables.Add(selectable);
        }
        return selectables;
    }

    public virtual void AddSelectable(Selectable value)
    {
        selectables?.Add(value);
    }

    public virtual void ClearInteractables()
    {
        selectables?.Clear();
    }

    protected virtual List<Selectable> RemoveUninteractables(List<Selectable> selectables)
    {
        List<Selectable> tempSelectables = new List<Selectable>(selectables);

        for (int i = 0; i < tempSelectables.Count; i++)
        {
            if (!tempSelectables[i].interactable)
            {
                tempSelectables.RemoveAt(i);
                i--;
            }
        }

        return tempSelectables;
    }

    public virtual void FindNavigation()
    {
        selectables = RemoveUninteractables(FindSelectables());
    }
}
