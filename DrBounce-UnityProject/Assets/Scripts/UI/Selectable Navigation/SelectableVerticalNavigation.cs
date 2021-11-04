using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableVerticalNavigation : SelectableNavigation
{
    private void OnEnable()
    {
        FindNavigation(RemoveUninteractables(selectables));
    }

    protected override void FindNavigation(List<Selectable> selectables)
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
