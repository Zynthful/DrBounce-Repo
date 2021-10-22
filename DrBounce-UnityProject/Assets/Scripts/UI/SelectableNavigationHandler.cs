using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableNavigationHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The selectable objects that will have its navigations set. NOTE: This should be ordered top to bottom.")]
    protected List<Selectable> selectables = null;

    private void OnEnable()
    {
        FindNavigation(RemoveUninteractables(selectables));
    }

    public void AddSelectable(Selectable value)
    {
        selectables.Add(value);
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

    public virtual void FindNavigation(List<Selectable> selectables)
    {
        for (int i = 0; i < selectables.Count; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;

            nav.selectOnUp = i == 0 ? selectables[selectables.Count - 1] : selectables[i - 1];
            nav.selectOnDown = i == selectables.Count - 1 ? selectables[0] : selectables[i + 1];

            selectables[i].navigation = nav;
        }
    }
}
