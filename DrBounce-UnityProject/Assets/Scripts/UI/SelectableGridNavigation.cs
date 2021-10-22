using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableGridNavigation : SelectableNavigationHandler
{
    [SerializeField]
    private GridLayoutGroup grid = null;

    // private int constraintCount = 0;

    [SerializeField]
    private int rowCount;
    [SerializeField]
    private int columnCount;

    private void OnEnable()
    {
        /*
        if (grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount || grid.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            constraintCount = grid.constraintCount;
        }
        */
        FindNavigation(RemoveUninteractables(selectables));
    }

    // to do: dynamically find row and column count rather than having to input it in inspector
    private void GetRowAndColumnCount()
    {

    }

    // to do: make this not copy paste code from SelectableNavigationHandler
    public override void FindNavigation(List<Selectable> selectables)
    {
        for (int j = 0; j < rowCount; j++)
        {
            for (int i = 0; i < columnCount; i++)
            {
                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.Explicit;

                // Is the current cell on the last row?
                // TRUE: loop back to first row
                // FALSE: go to cell on the next row
                nav.selectOnDown = j == rowCount - 1 ? selectables[i] : selectables[i + (j * columnCount) + columnCount];

                // Is the current cell on the first row?
                // TRUE: loop to last row
                // FALSE: go to cell on the previous row
                nav.selectOnUp = j == 0 ? selectables[i + (columnCount * (rowCount - 1))] : selectables[i + (j * columnCount) - columnCount];

                selectables[i + (j * columnCount)].navigation = nav;
            }
        }
    }
}
