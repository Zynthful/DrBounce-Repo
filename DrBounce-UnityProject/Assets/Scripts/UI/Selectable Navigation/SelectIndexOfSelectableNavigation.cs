using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectIndexOfSelectableNavigation : SelectableNavigation
{
    [SerializeField]
    protected SelectableNavigation selectableNav;     // The navigation we use for overriding the selectable

    [SerializeField]
    protected int selectOnUp = -1;
    [SerializeField]
    protected int selectOnDown = -1;
    [SerializeField]
    protected int selectOnLeft = -1;
    [SerializeField]
    protected int selectOnRight = -1;

    public override void FindNavigation()
    {
        base.FindNavigation();

        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;

        // help.
        if (selectOnUp >= 0)
        {
            nav.selectOnUp = selectableNav.GetSelectables()[selectOnUp];
        }
        if (selectOnDown >= 0)
        {
            nav.selectOnDown = selectableNav.GetSelectables()[selectOnDown];
        }
        if (selectOnLeft >= 0)
        {
            nav.selectOnLeft = selectableNav.GetSelectables()[selectOnLeft];
        }
        if (selectOnRight >= 0)
        {
            nav.selectOnRight = selectableNav.GetSelectables()[selectOnRight];
        }

        // Apply our new navigation to all our selectables that we want to override
        for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].navigation = nav;
        }
    }
}