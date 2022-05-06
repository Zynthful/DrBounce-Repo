using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class SelectableNavigation : MonoBehaviour
{
    protected List<Selectable> selectables = null;
    public List<Selectable> GetSelectables() { return selectables; }

    [SerializeField]
    protected bool autoFindNavigation = true;

    protected virtual void Start()
    {
        #if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            selectables = RemoveUninteractables(FindSelectables());
            if (autoFindNavigation)
            {
                FindNavigation();
            }
        }
#else
        if (autoFindNavigation)
        {
            selectables = RemoveUninteractables(FindSelectables());
            FindNavigation();
        }
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

    }

    public void SelectIndex(int index)
    {
        selectables[index].Select();
    }

    public void SelectFirst()
    {
        SelectIndex(0);
    }

    public void SelectLast()
    {
        SelectIndex(selectables.Count - 1);
    }
}