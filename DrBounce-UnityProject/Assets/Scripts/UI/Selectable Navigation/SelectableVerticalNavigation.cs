using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SelectableVerticalNavigation : SelectableNavigation
{
    public override void FindNavigation()
    {
        base.FindNavigation();
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