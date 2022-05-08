using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SelectableGridNavigation : SelectableNavigation
{
    [Header("Navigation Loop Settings")]
    [SerializeField]
    private bool topLoopToBottom = true;   // if we press up whislt on the top cell, should it loop back to the bottom cell?
    [SerializeField]
    private bool bottomLoopToTop = true;   // if we press down whilst on the bottom cell, should it loop back to the top cell?
    [SerializeField]
    private bool leftLoopToRight = true;   // if we press left whilst on the left-most cell, should it loop back to the right-most cell?
    [SerializeField]
    private bool rightLoopToLeft = true;   // if we press right whilst on the right-most cell, should it loop back to the left-most cell?

    [Header("Row and Column Settings")]
    [SerializeField]
    private int rowCount;
    [SerializeField]
    private int columnCount;

    // to do: dynamically find row and column count rather than having to input it in inspector
    private void GetRowAndColumnCount()
    {

    }

    public override void FindNavigation()
    {
        base.FindNavigation();
        for (int j = 0; j < rowCount; j++)
        {
            for (int i = 0; i < columnCount; i++)
            {
                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.Explicit;
                int currentCell = i + (j * columnCount);

                // Is the current cell on the last row?
                if (j == rowCount - 1)
                {
                    // Loop to top row OR keep current nav
                    nav.selectOnDown = bottomLoopToTop ? selectables[i] : selectables[currentCell].navigation.selectOnDown;
                }
                // Go to cell on the next row
                else
                {
                    nav.selectOnDown = selectables[currentCell + columnCount];
                }

                // Is the current cell on the first row?
                if (j == 0)
                {
                    // Loop to top row OR keep current nav
                    nav.selectOnUp = topLoopToBottom ? selectables[i + (columnCount * (rowCount - 1))] : selectables[currentCell].navigation.selectOnUp;
                }
                // Go to cell on the previous row
                else
                {
                    nav.selectOnUp = selectables[currentCell - columnCount];
                }

                // Is the current cell on the left-most column?
                if (i == 0)
                {
                    // Loop to right most column OR keep current nav
                    nav.selectOnLeft = leftLoopToRight ? selectables[columnCount - 1 + currentCell] : selectables[currentCell].navigation.selectOnLeft;
                }
                // Go to cell on the previous column
                else
                {
                    nav.selectOnLeft = selectables[currentCell - 1];
                }

                // Is the current cell on the right-most column?
                if (i == columnCount - 1)
                {
                    // Loop to left-most column OR keep current nav
                    nav.selectOnRight = rightLoopToLeft ? selectables[currentCell - i] : selectables[currentCell].navigation.selectOnRight;
                }
                // Go to cell on the next column
                else
                {
                    nav.selectOnRight = selectables[currentCell + 1];
                }

                /*
                nav.selectOnDown = j == rowCount - 1 ? selectables[i] : selectables[currentCell + columnCount];
                nav.selectOnUp = j == 0 ? selectables[i + (columnCount * (rowCount - 1))] : selectables[currentCell - columnCount];
                nav.selectOnLeft = i == 0 ? selectables[columnCount - 1 + currentCell] : selectables[currentCell - 1];
                nav.selectOnRight = i == columnCount - 1 ? selectables[currentCell - i] : selectables[currentCell + 1];
                */

                selectables[currentCell].navigation = nav;
            }
        }
    }
}
