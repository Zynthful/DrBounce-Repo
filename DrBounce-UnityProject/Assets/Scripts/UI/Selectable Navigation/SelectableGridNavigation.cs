using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableGridNavigation : SelectableNavigationHandler
{
    [SerializeField]
    private int rowCount;
    [SerializeField]
    private int columnCount;

    private void OnEnable()
    {
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

                // Is the current cell on the left-most column?
                // TRUE: loop to right-most column
                // FALSE: go to cell on the previous column
                nav.selectOnLeft = i == 0 ? selectables[columnCount - 1 + i + (j * columnCount)] : selectables[i + (j * columnCount) - 1];

                // Is the current cell on the right-most column?
                // TRUE: loop to left-most column
                // FALSE: go to cell on the next column
                nav.selectOnRight = i == columnCount - 1 ? selectables[j * columnCount] : selectables[i + (j * columnCount) + 1];

                selectables[i + (j * columnCount)].navigation = nav;
            }
        }
    }
}
