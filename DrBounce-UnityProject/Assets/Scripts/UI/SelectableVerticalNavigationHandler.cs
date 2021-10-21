using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableVerticalNavigationHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The selectable objects that will have its navigations set. NOTE: This should be ordered top to bottom.")]
    private Selectable[] selectables = null;

    private void OnEnable()
    {
        FindVerticalNavigation(RemoveUninteractables(selectables));
    }

    private Selectable[] RemoveUninteractables(Selectable[] selectables)
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

        return tempSelectables.ToArray();
    }

    private void FindVerticalNavigation(Selectable[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;

            nav.selectOnUp = i == 0 ? buttons[buttons.Length - 1] : buttons[i - 1];
            nav.selectOnDown = i == buttons.Length - 1 ? buttons[0] : buttons[i + 1];

            buttons[i].navigation = nav;
        }
    }
}
